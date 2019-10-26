using System.Threading.Tasks;
using Doug.Controllers;
using Doug.Repositories;
using Doug.Slack;

namespace Doug.Services.MenuServices
{
    public interface IPartyMenuService
    { 
        Task AcceptInvite(Interaction interaction);
        Task RefuseInvite(Interaction interaction);
    }

    public class PartyMenuService : IPartyMenuService
    {
        private readonly ISlackWebApi _slack;
        private readonly IPartyService _partyService;
        private readonly IUserRepository _userRepository;

        public PartyMenuService(ISlackWebApi slack, IPartyService partyService, IUserRepository userRepository)
        {
            _slack = slack;
            _partyService = partyService;
            _userRepository = userRepository;
        }

        public async Task AcceptInvite(Interaction interaction)
        {
            var user = _userRepository.GetUser(interaction.UserId);

            var response = _partyService.AcceptInvite(int.Parse(interaction.Value), user);

            await _slack.SendEphemeralMessage(response.Message, user.Id, interaction.ChannelId);
            await _slack.DeleteInteractionMessage(interaction.ResponseUrl);
        }

        public async Task RefuseInvite(Interaction interaction)
        {
            await _slack.DeleteInteractionMessage(interaction.ResponseUrl);
        }
    }
}
