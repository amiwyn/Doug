using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Doug.Effects;
using Doug.Effects.Buffs;
using Doug.Models.Monsters;
using Doug.Models.User;
using Doug.Repositories;
using Doug.Slack;

namespace Doug.Services
{
    public interface IUserService
    {
        string Mention(User user);
        Task<bool> ApplyTrueDamage(User user, int damage, string channel);
        Task AddExperience(User user, long experience, string channel);
        Task AddBulkExperience(List<User> users, long experience, string channel);
        Task AddExperienceFromMonster(List<User> users, Monster monster, string channel);
        Task<bool> IsUserActive(string userId);
        Task KillUser(User user, string channel);
        Task<bool> HandleDeath(User user, string channel);
    }

    public class UserService : IUserService
    {
        private readonly ISlackWebApi _slack;
        private readonly IStatsRepository _statsRepository;
        private readonly IEventDispatcher _eventDispatcher;
        private readonly IEffectRepository _effectRepository;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IItemRepository _itemRepository;

        private const string MysteryBoxId = "mystery_box";

        public UserService(ISlackWebApi slack, IStatsRepository statsRepository, IEventDispatcher eventDispatcher, IEffectRepository effectRepository, IInventoryRepository inventoryRepository, IItemRepository itemRepository)
        {
            _slack = slack;
            _statsRepository = statsRepository;
            _eventDispatcher = eventDispatcher;
            _effectRepository = effectRepository;
            _inventoryRepository = inventoryRepository;
            _itemRepository = itemRepository;
        }

        public string Mention(User user)
        {
            return _eventDispatcher.OnMention(user, $"<@{user.Id}>");
        }

        public async Task<bool> ApplyTrueDamage(User user, int damage, string channel)
        {
            user.Health -= damage;

            if (user.IsDead())
            {
                return await HandleDeath(user, channel);
            }

            _statsRepository.UpdateHealth(user.Id, user.Health);
            return false;
        }

        public async Task<bool> HandleDeath(User user, string channel)
        {
            if (!_eventDispatcher.OnDeath(user))
            {
                return false;
            }

            _statsRepository.KillUser(user.Id);
            _effectRepository.AddEffect(user, MortuaryGrace.EffectId, 15);

            await _slack.BroadcastMessage(string.Format(DougMessages.UserDied, Mention(user)), channel);
            await _slack.KickUser(user.Id, channel);
            return true;
        }

        public async Task AddExperience(User user, long experience, string channel)
        {
            await AddBulkExperience(new List<User> { user }, experience, channel);
        }

        public async Task AddBulkExperience(List<User> users, long experience, string channel)
        {
            var levels = users.ToDictionary(user => user.Id, user => user.Level);

            var userIds = users.Select(user => user.Id).ToList();
            _statsRepository.AddExperienceToUsers(userIds, experience);

            var expGainMessageTasks = users.Select(user => _slack.SendEphemeralMessage(string.Format(DougMessages.GainedExp, experience), user.Id, channel));

            var levelUpUsers = users.Where(user => levels.GetValueOrDefault(user.Id) < user.Level).ToList();

            LevelUpUsers(levelUpUsers);

            var levelUpMessageTasks = levelUpUsers.Select(user => _slack.BroadcastMessage(string.Format(DougMessages.LevelUp, Mention(user), user.Level), channel));

            await Task.WhenAll(expGainMessageTasks);
            await Task.WhenAll(levelUpMessageTasks);
        }

        public async Task AddExperienceFromMonster(List<User> users, Monster monster, string channel)
        {
            var levels = users.ToDictionary(user => user.Id, user => user.Level);

            var userIds = users.Select(user => user.Id).ToList();
            _statsRepository.AddMonsterExperienceToUsers(userIds, monster);

            var expGainMessageTasks = users.Select(user => _slack.SendEphemeralMessage(string.Format(DougMessages.GainedExp, user.CalculateExperienceGainedFromMonster(monster, users.Count)), user.Id, channel));

            var levelUpUsers = users.Where(user => levels.GetValueOrDefault(user.Id) < user.Level).ToList();

            LevelUpUsers(levelUpUsers);

            var levelUpMessageTasks = levelUpUsers.Select(user => _slack.BroadcastMessage(string.Format(DougMessages.LevelUp, Mention(user), user.Level), channel));

            await Task.WhenAll(levelUpMessageTasks.Concat(expGainMessageTasks));
        }

        private void LevelUpUsers(List<User> users)
        {
            var mysteryBox = _itemRepository.GetItem(MysteryBoxId);
            _statsRepository.LevelUpUsers(users.Select(user => user.Id).ToList());
            _inventoryRepository.AddItemToUsers(users, mysteryBox);
        }

        public Task<bool> IsUserActive(string userId)
        {
            return _slack.GetUserPresence(userId);
        }

        public async Task KillUser(User user, string channel)
        {
            await HandleDeath(user, channel);
        }
    }
}
