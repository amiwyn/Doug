using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Doug.Items;
using Doug.Models;
using Doug.Repositories;
using Doug.Slack;

namespace Doug.Services
{
    public interface IUserService
    {
        string Mention(User user);
        Task<bool> RemoveHealth(User user, int health, string channel);
        Task AddExperience(User user, long experience, string channel);
        Task AddBulkExperience(List<User> users, long experience, string channel);
    }

    public class UserService : IUserService
    {
        private readonly ISlackWebApi _slack;
        private readonly IStatsRepository _statsRepository;
        private readonly IEventDispatcher _eventDispatcher;

        public UserService(ISlackWebApi slack, IStatsRepository statsRepository, IEventDispatcher eventDispatcher)
        {
            _slack = slack;
            _statsRepository = statsRepository;
            _eventDispatcher = eventDispatcher;
        }

        public string Mention(User user)
        {
            return _eventDispatcher.OnMention(user, $"<@{user.Id}>");
        }

        public async Task<bool> RemoveHealth(User user, int health, string channel)
        {
            user.Health -= health;

            if (user.IsDead())
            {
                if (!_eventDispatcher.OnDeath(user))
                {
                    return false;
                }

                _statsRepository.KillUser(user.Id);
                await _slack.BroadcastMessage(string.Format(DougMessages.UserDied, Mention(user)), channel);
                await _slack.KickUser(user.Id, channel);
                return true;
            }

            _statsRepository.UpdateHealth(user.Id, user.Health);
            return false;
        }

        public async Task AddExperience(User user, long experience, string channel)
        {
            await AddBulkExperience(new List<User> {user}, experience, channel);
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
    }
}
