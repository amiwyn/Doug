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
        Task HandleMonsterDeathByUser(User user, SpawnedMonster spawnedMonster, string channel);
    }

    public class MonsterService : IMonsterService
    {
        private readonly IMonsterRepository _monsterRepository;
        private readonly ISlackWebApi _slack;
        private readonly IUserService _userService;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IRandomService _randomService;

        public MonsterService(IMonsterRepository monsterRepository, ISlackWebApi slack, IUserService userService, IInventoryRepository inventoryRepository, IRandomService randomService)
        {
            _monsterRepository = monsterRepository;
            _slack = slack;
            _userService = userService;
            _inventoryRepository = inventoryRepository;
            _randomService = randomService;
        }

        public async Task HandleMonsterDeathByUser(User user, SpawnedMonster spawnedMonster, string channel)
        {
            var monster = spawnedMonster.Monster;
            var users = spawnedMonster.MonsterAttackers.Select(attacker => attacker.User).ToList();
            var lootWinner = spawnedMonster.FindHighestDamageDealer();

            await AddMonsterLootToUser(lootWinner, monster, channel);

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
