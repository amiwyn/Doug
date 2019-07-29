using System;
using System.Linq;
using System.Threading.Tasks;
using Doug.Items;
using Doug.Models;
using Doug.Monsters;
using Doug.Repositories;
using Doug.Slack;

namespace Doug.Services
{
    public interface IMonsterService
    {
        Task RollMonsterSpawn();
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
        private readonly IUserRepository _userRepository;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IRandomService _randomService;
        private readonly IItemFactory _itemFactory;
        private readonly IChannelRepository _channelRepository;
        private readonly IMonsterFactory _monsterFactory;

        public MonsterService(IMonsterRepository monsterRepository, ISlackWebApi slack, IUserService userService, IUserRepository userRepository, IInventoryRepository inventoryRepository, IRandomService randomService, IItemFactory itemFactory, IChannelRepository channelRepository, IMonsterFactory monsterFactory)
        {
            _monsterRepository = monsterRepository;
            _slack = slack;
            _userService = userService;
            _userRepository = userRepository;
            _inventoryRepository = inventoryRepository;
            _randomService = randomService;
            _itemFactory = itemFactory;
            _channelRepository = channelRepository;
            _monsterFactory = monsterFactory;
        }

        public async Task RollMonsterSpawn()
        {
            var random = new Random();
            if (random.NextDouble() >= SpawnChance)
            {
                return;
            }

            var channel = PvpChannel;
            if (random.Next(2) == 0)
            {
                channel = PickRandomChannel(random).Id;
            }

            var monster = _monsterFactory.CreateRandomMonster(random);
            var monstersInChannel = _monsterRepository.GetMonsters(channel);

            if (monstersInChannel.Count(monsta => monsta.MonsterId == monster.Id) >= MaximumMonsterTypeInChannel)
            {
                return;
            }

            _monsterRepository.SpawnMonster(monster, channel); 
            await _slack.BroadcastMessage(string.Format(DougMessages.MonsterSpawned, monster.Name), channel);
        }

        private Channel PickRandomChannel(Random random)
        {
            var channels = _channelRepository.GetChannels().ToList();
            var index = random.Next(0, channels.Count);
            return channels.ElementAt(index);
        }

        public async Task HandleMonsterDeathByUser(User user, SpawnedMonster spawnedMonster, string channel)
        {
            var monster = spawnedMonster.Monster;
            var userIds = await _slack.GetUsersInChannel(channel);
            var users = _userRepository.GetUsers(userIds);

            await AddMonsterLootToUser(user, monster, channel);

            _monsterRepository.RemoveMonster(spawnedMonster.Id);

            await _slack.BroadcastMessage(string.Format(DougMessages.MonsterDied, monster.Name), channel);

            await _userService.AddBulkExperience(users, monster.ExperienceValue, channel);
        }

        private async Task AddMonsterLootToUser(User user, Monster monster, string channel)
        {
            var droppedItems = _randomService.RandomTableDrop(monster.DropTable, user.ExtraDropChance()).Select(drop => _itemFactory.CreateItem(drop.Id)).ToList();

            if (!droppedItems.Any())
            {
                _inventoryRepository.AddItems(user, droppedItems);
                var itemsMessage = string.Join(", ", droppedItems.Select(item => $"*{item.Name}*"));
                await _slack.BroadcastMessage(string.Format(DougMessages.UserObtained, _userService.Mention(user), itemsMessage), channel);
            }
        }
    }
}
