using System.Threading.Tasks;
using Doug.Menus;
using Doug.Models;
using Doug.Repositories;
using Doug.Slack;

namespace Doug.Services.MenuServices
{
    public interface IStatsMenuService
    {
        Task AttributeStatPoint(Interaction interaction);
    }

    public class StatsMenuService : IStatsMenuService
    {
        private readonly IUserRepository _userRepository;
        private readonly IStatsRepository _statsRepository;
        private readonly ISlackWebApi _slack;

        public StatsMenuService(IStatsRepository statsRepository, IUserRepository userRepository, ISlackWebApi slack)
        {
            _statsRepository = statsRepository;
            _userRepository = userRepository;
            _slack = slack;
        }

        public async Task AttributeStatPoint(Interaction interaction)
        {
            var user = _userRepository.GetUser(interaction.UserId);

            if (user.FreeStatsPoints <= 0)
            {
                await _slack.SendEphemeralMessage(DougMessages.NoMoreStatsPoints, user.Id, interaction.ChannelId);
                return;
            }

            _statsRepository.AttributeStatPoint(user.Id, interaction.Value);

            await _slack.UpdateInteractionMessage(new StatsMenu(user).Blocks, interaction.ResponseUrl);
        }
    }
}
