using System;
using System.Linq;
using System.Threading.Tasks;
using Doug.Models;
using Doug.Monsters;
using Doug.Repositories;
using Doug.Slack;

namespace Doug.Services
{
    public interface IMonsterService
    {
        void RollMonsterSpawn();
        Task HandleMonsterDeathByUser(User user, SpawnedMonster spawnedMonster, string channel);
    }

    public class MonsterService : IMonsterService
    {
        private const double SpawnChance = 0.2;
        private const string PvpChannel = "CL2TYGE1E";
        private const int MaximumMonsterTypeInChannel = 3;

        private readonly IMonsterRepository _monsterRepository;
        private readonly ISlackWebApi _slack;
        private readonly IUserService _userService;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IRandomService _randomService;
        private readonly IMonsterFactory _monsterFactory;
        private readonly IChannelRepository _channelRepository;

        public MonsterService(IMonsterRepository monsterRepository, ISlackWebApi slack, IUserService userService, IInventoryRepository inventoryRepository, IRandomService randomService, IMonsterFactory monsterFactory, IChannelRepository channelRepository)
        {
            _monsterRepository = monsterRepository;
            _slack = slack;
            _userService = userService;
            _inventoryRepository = inventoryRepository;
            _randomService = randomService;
            _monsterFactory = monsterFactory;
            _channelRepository = channelRepository;
        }

        public void RollMonsterSpawn()
        {
            var random = new Random();
            if (random.NextDouble() >= SpawnChance)
            {
                return;
            }

            var channel = PvpChannel;
            //if (random.Next(2) == 0)
            //{
            //    channel = PickRandomChannel(random, _channelRepository).Id;
            //}

            var monster = _monsterFactory.CreateRandomMonster(random);
            var monstersInChannel = _monsterRepository.GetMonsters(channel);

            if (monstersInChannel.Count(monsta => monsta.MonsterId == monster.Id) >= MaximumMonsterTypeInChannel)
            {
                return;
            }

            _monsterRepository.SpawnMonster(monster, channel);
            _slack.BroadcastMessage(string.Format(DougMessages.MonsterSpawned, monster.Name), channel).Wait();
        }

        private Channel PickRandomChannel(Random random, IChannelRepository channelRepository)
        {
            var channels = channelRepository.GetChannels().ToList();
            var index = random.Next(0, channels.Count);
            return channels.ElementAt(index);
        }

        public async Task HandleMonsterDeathByUser(User user, SpawnedMonster spawnedMonster, string channel)
        {
            var monster = spawnedMonster.Monster;
            var users = spawnedMonster.MonsterAttackers.Select(attacker => attacker.User).ToList();

            await AddMonsterLootToUser(user, monster, channel);

            _monsterRepository.RemoveMonster(spawnedMonster.Id);

            await _slack.BroadcastMessage(string.Format(DougMessages.MonsterDied, monster.Name), channel);

            var experiencePerUser = monster.ExperienceValue / users.Count;
            await _userService.AddBulkExperience(users, experiencePerUser, channel);
        }

        private async Task AddMonsterLootToUser(User user, Monster monster, string channel)
        {
            var droppedItems = _randomService.RandomTableDrop(monster.DropTable, user.ExtraDropChance()).Select(drop => drop.Item).ToList();

            if (droppedItems.Any())
            {
                _inventoryRepository.AddItems(user, droppedItems);
                var itemsMessage = string.Join(", ", droppedItems.Select(item => $"*{item.Name}*"));
                await _slack.BroadcastMessage(string.Format(DougMessages.UserObtained, _userService.Mention(user), itemsMessage), channel);
            }
        }
    }
}
