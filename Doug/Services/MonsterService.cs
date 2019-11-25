using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Doug.Models;
using Doug.Models.Monsters;
using Doug.Models.User;
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
        private const int MaximumMonsterTypeInChannel = 3;

        private readonly ISpawnedMonsterRepository _spawnedMonsterRepository;
        private readonly ISlackWebApi _slack;
        private readonly IUserService _userService;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IRandomService _randomService;
        private readonly IMonsterRepository _monsterRepository;
        private readonly IChannelRepository _channelRepository;
        private readonly IPartyRepository _partyRepository;
        private readonly IItemRepository _itemRepository;

        public MonsterService(ISpawnedMonsterRepository spawnedMonsterRepository, ISlackWebApi slack, IUserService userService, IInventoryRepository inventoryRepository, IRandomService randomService, IMonsterRepository monsterRepository, IChannelRepository channelRepository, IPartyRepository partyRepository, IItemRepository itemRepository)
        {
            _spawnedMonsterRepository = spawnedMonsterRepository;
            _slack = slack;
            _userService = userService;
            _inventoryRepository = inventoryRepository;
            _randomService = randomService;
            _monsterRepository = monsterRepository;
            _channelRepository = channelRepository;
            _partyRepository = partyRepository;
            _itemRepository = itemRepository;
        }

        public void RollMonsterSpawn()
        {
            var channel = PickRandomRegion(_channelRepository);
            var monsterIds = channel.Monsters.Select(mons => mons.MonsterId).ToList().OrderBy(mons => Guid.NewGuid());
            var monstersInChannel = _spawnedMonsterRepository.GetMonsters(channel.Id).ToList();

            foreach (var monsterId in monsterIds)
            {
                if (monstersInChannel.Count(monsta => monsta.MonsterId == monsterId) < MaximumMonsterTypeInChannel)
                {
                    var monster = _monsterRepository.GetMonster(monsterId);
                    _spawnedMonsterRepository.SpawnMonster(monster, channel.Id);
                    _slack.BroadcastMessage(string.Format(DougMessages.MonsterSpawned, monster.Name), channel.Id).Wait();
                    return;
                }
            }
        }

        private Channel PickRandomRegion(IChannelRepository channelRepository)
        {
            var channels = channelRepository.GetChannelsByType(ChannelType.Region).ToList();
            var index = new Random().Next(0, channels.Count);
            return channels.ElementAt(index);
        }

        public async Task HandleMonsterDeathByUser(User user, SpawnedMonster spawnedMonster, string channel)
        {
            var monster = spawnedMonster.Monster;
            var users = FindExperienceWinners(spawnedMonster.MonsterAttackers);

            await AddMonsterLootToUser(user, monster, channel);

            _spawnedMonsterRepository.RemoveMonster(spawnedMonster.Id);

            await _slack.BroadcastMessage(string.Format(DougMessages.MonsterDied, monster.Name), channel);

            await _userService.AddExperienceFromMonster(users, monster, channel);
        }

        private List<User> FindExperienceWinners(List<MonsterAttacker> attackers)
        {
            var users = attackers.Select(attacker => attacker.User.Id);
            var parties = _partyRepository.GetUniquePartiesFromUsers(users);

            var usersWithoutParty = attackers.Where(attacker => !parties.Any(party => party.Users.Select(user => user.Id).Contains(attacker.UserId))).ToList();
            var mostSoloDps = usersWithoutParty.Aggregate(new MonsterAttacker(), (acc, elem) => acc.DamageDealt > elem.DamageDealt ? acc : elem);

            if (parties.Any())
            {
                var winningParty = parties.Select(party => new { Damage = GetPartyDamage(party, attackers), Party = party })
                    .Aggregate((acc, elem) => acc.Damage > elem.Damage ? acc : elem);

                if (winningParty.Damage >= mostSoloDps.DamageDealt)
                {
                    return winningParty.Party.Users;
                }
            }

            return new List<User> { mostSoloDps.User };
        }

        private int GetPartyDamage(Party party, List<MonsterAttacker> attackers)
        {
            return party.Users.Sum(user => attackers.SingleOrDefault(atk => atk.User.Id == user.Id)?.DamageDealt ?? 0);
        }

        private async Task AddMonsterLootToUser(User user, Monster monster, string channel)
        {
            var droppedItems = _randomService.RandomTableDrop(monster.DropTable, user.ExtraDropChance()).Select(drop => drop.Id).ToList();

            var items = _itemRepository.GetItems(droppedItems);

            if (droppedItems.Any())
            {
                _inventoryRepository.AddItems(user, items);
                var itemsMessage = string.Join(", ", items.Select(item => $"{item.GetDisplayName()}"));
                await _slack.BroadcastMessage(string.Format(DougMessages.UserObtained, _userService.Mention(user), itemsMessage), channel);
            }
        }
    }
}
