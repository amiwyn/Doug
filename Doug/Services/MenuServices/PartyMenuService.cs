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
        private readonly IPartyRepository _partyRepository;
        private readonly IUserService _userService;

        public PartyMenuService(ISlackWebApi slack, IPartyService partyService, IUserRepository userRepository, IPartyRepository partyRepository, IUserService userService)
        {
            _slack = slack;
            _partyService = partyService;
            _userRepository = userRepository;
            _partyRepository = partyRepository;
            _userService = userService;
        }

        public async Task AcceptInvite(Interaction interaction)
        {
            var party = _partyRepository.GetParty(int.Parse(interaction.Value));
            var user = _userRepository.GetUser(interaction.UserId);

            var response = _partyService.AcceptInvite(party, user);

            await _slack.SendEphemeralMessage(string.Format(DougMessages.JoinedYourParty, _userService.Mention(user)), party.UserId, interaction.ChannelId);
            await _slack.SendEphemeralMessage(response.Message, user.Id, interaction.ChannelId);
            await _slack.DeleteInteractionMessage(interaction.ResponseUrl);
        }

        public async Task RefuseInvite(Interaction interaction)
        {
            await _slack.DeleteInteractionMessage(interaction.ResponseUrl);
        }
    }
}
