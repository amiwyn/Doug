using System;
using System.Linq;
using System.Threading.Tasks;
using Doug.Items;
using Doug.Models;
using Doug.Monsters;
using Doug.Monsters.Seagulls;
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

        private readonly IMonsterRepository _monsterRepository;
        private readonly ISlackWebApi _slack;
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IRandomService _randomService;
        private readonly IItemFactory _itemFactory;

        public MonsterService(IMonsterRepository monsterRepository, ISlackWebApi slack, IUserService userService, IUserRepository userRepository, IInventoryRepository inventoryRepository, IRandomService randomService, IItemFactory itemFactory)
        {
            _monsterRepository = monsterRepository;
            _slack = slack;
            _userService = userService;
            _userRepository = userRepository;
            _inventoryRepository = inventoryRepository;
            _randomService = randomService;
            _itemFactory = itemFactory;
        }

        public void RollMonsterSpawn()
        {
            if (new Random().NextDouble() >= SpawnChance)
            {
                return;
            }

            var monster = new Seagull(); //TODO add more monster variety and pick them randomly (or based on present players levels)

            _monsterRepository.SpawnMonster(monster, PvpChannel); 
            _slack.BroadcastMessage(string.Format(DougMessages.MonsterSpawned, monster.Name), PvpChannel);
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
                await _slack.SendEphemeralMessage(string.Format(DougMessages.UserObtained, _userService.Mention(user), itemsMessage), user.Id, channel);
            }
        }
    }
}
