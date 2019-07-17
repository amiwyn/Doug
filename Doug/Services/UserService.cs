using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Doug.Effects.Buffs;
using Doug.Items;
using Doug.Models;
using Doug.Repositories;
using Doug.Slack;

namespace Doug.Services
{
    public interface IUserService
    {
        string Mention(User user);
        Task<bool> ApplyMagicalDamage(User user, int damage, string channel);
        Task AddExperience(User user, long experience, string channel);
        Task AddBulkExperience(List<User> users, long experience, string channel);
        Task<int> PhysicalAttack(User user, User target, string channel);
        Task<bool> IsUserActive(string userId);
    }

    public class UserService : IUserService
    {
        private const int KillExperienceGain = 100;

        private readonly ISlackWebApi _slack;
        private readonly IStatsRepository _statsRepository;
        private readonly IEventDispatcher _eventDispatcher;
        private readonly IEffectRepository _effectRepository;

        public UserService(ISlackWebApi slack, IStatsRepository statsRepository, IEventDispatcher eventDispatcher, IEffectRepository effectRepository)
        {
            _slack = slack;
            _statsRepository = statsRepository;
            _eventDispatcher = eventDispatcher;
            _effectRepository = effectRepository;
        }

        public string Mention(User user)
        {
            return _eventDispatcher.OnMention(user, $"<@{user.Id}>");
        }

        public async Task<bool> ApplyMagicalDamage(User user, int damage, string channel)
        {
            user.Health -= damage;

            if (user.IsDead())
            {
                return await HandleDeath(user, channel);
            }

            _statsRepository.UpdateHealth(user.Id, user.Health);
            return false;
        }

        private async Task<bool> HandleDeath(User user, string channel)
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

            _statsRepository.LevelUpUsers(levelUpUsers.Select(user => user.Id).ToList());

            var levelUpMessageTasks = levelUpUsers.Select(user => _slack.BroadcastMessage(string.Format(DougMessages.LevelUp, Mention(user), user.Level), channel));

            await Task.WhenAll(expGainMessageTasks);
            await Task.WhenAll(levelUpMessageTasks);
        }

        public async Task<int> PhysicalAttack(User user, User target, string channel)
        {
            var damageDealt = user.AttackUser(target, _eventDispatcher);

            if (target.IsDead() && await HandleDeath(target, channel))
            {
                _eventDispatcher.OnDeathByUser(target, user);
                await AddExperience(user, KillExperienceGain, channel);
            }

            _statsRepository.UpdateHealth(target.Id, target.Health);
            return damageDealt;
        }

        public Task<bool> IsUserActive(string userId)
        {
            return _slack.GetUserPresence(userId);
        }
    }
}
