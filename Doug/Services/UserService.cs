using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Doug.Models;
using Doug.Repositories;
using Doug.Slack;

namespace Doug.Services
{
    public interface IUserService
    {
        Task RemoveHealth(User user, int health, string channel);
        Task AddExperience(User user, long experience, string channel);
        Task AddBulkExperience(List<User> users, long experience, string channel);
    }

    public class UserService : IUserService
    {
        private readonly ISlackWebApi _slack;
        private readonly IStatsRepository _statsRepository;

        public UserService(ISlackWebApi slack, IStatsRepository statsRepository)
        {
            _slack = slack;
            _statsRepository = statsRepository;
        }

        public async Task RemoveHealth(User user, int health, string channel)
        {
            user.Health -= health;

            if (user.IsDead())
            {
                _statsRepository.KillUser(user.Id);
                await _slack.BroadcastMessage(string.Format(DougMessages.UserDied, Utils.UserMention(user.Id)), channel);
            }
            else
            {
                _statsRepository.UpdateHealth(user.Id, user.Health);
            }
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

            var levelUpMessageTasks = levelUpUsers.Select(user => _slack.BroadcastMessage(string.Format(DougMessages.LevelUp, Utils.UserMention(user.Id), user.Level), channel));

            await Task.WhenAll(expGainMessageTasks);
            await Task.WhenAll(levelUpMessageTasks);
        }
    }
}
