using System.Threading.Tasks;
using Doug.Controllers;
using Doug.Menus;
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
        private readonly IPartyRepository _partyRepository;

        public StatsMenuService(IStatsRepository statsRepository, IUserRepository userRepository, ISlackWebApi slack, IPartyRepository partyRepository)
        {
            _statsRepository = statsRepository;
            _userRepository = userRepository;
            _slack = slack;
            _partyRepository = partyRepository;
        }

        public async Task AttributeStatPoint(Interaction interaction)
        {
            var user = _userRepository.GetUser(interaction.UserId);
            var party = _partyRepository.GetPartyByUser(user.Id);

            if (user.FreeStatsPoints <= 0)
            {
                await _slack.SendEphemeralMessage(DougMessages.NoMoreStatsPoints, user.Id, interaction.ChannelId);
                return;
            }

            _statsRepository.AttributeStatPoint(user.Id, interaction.Value);

            await _slack.UpdateInteractionMessage(new StatsMenu(user, party).Blocks, interaction.ResponseUrl);
        }
    }
}
