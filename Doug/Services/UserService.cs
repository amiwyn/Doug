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

            if (user.Health <= 0)
            {
                await _slack.BroadcastMessage(string.Format(DougMessages.UserDied, Utils.UserMention(user.Id)), channel);
            }

            _statsRepository.UpdateHealth(user.Id, user.Health);
        }

        public async Task AddExperience(User user, long experience, string channel)
        {
            await AddExperienceToUser(user, experience, channel);

            _statsRepository.AddExperience(user.Id, experience);
        }

        private async Task AddExperienceToUser(User user, long experience, string channel)
        {
            var previousLevel = user.Level;
            user.Experience += experience;

            await _slack.SendEphemeralMessage(string.Format(DougMessages.GainedExp, experience), user.Id, channel);

            if (previousLevel < user.Level)
            {
                await _slack.BroadcastMessage(string.Format(DougMessages.LevelUp, Utils.UserMention(user.Id), user.Level), channel);
            }
        }

        public async Task AddBulkExperience(List<User> users, long experience, string channel)
        {
            var tasks = users.Select(user => AddExperienceToUser(user, experience, channel));

            var userIds = users.Select(user => user.Id).ToList();

            _statsRepository.AddExperienceToUsers(userIds, experience);

            await Task.WhenAll(tasks);
        }
    }
}
