using System.Threading.Tasks;
using Doug.Menus;
using Doug.Models;
using Doug.Models.User;
using Doug.Repositories;
using Doug.Slack;

namespace Doug.Services
{
    public interface IPartyService
    {
        Task<DougResponse> SendInvite(User target, User host, string channel);
        DougResponse AcceptInvite(int partyId, User user);
        DougResponse LeaveParty(User user);
    }

    public class PartyService : IPartyService
    {
        private readonly IPartyRepository _partyRepository;
        private readonly IUserService _userService;
        private readonly ISlackWebApi _slack;

        public PartyService(IPartyRepository partyRepository, IUserService userService, ISlackWebApi slack)
        {
            _partyRepository = partyRepository;
            _userService = userService;
            _slack = slack;
        }

        public DougResponse AcceptInvite(int partyId, User user)
        {
            var party = _partyRepository.GetParty(partyId);

            if (party.Users.Count >= 3)
            {
                return new DougResponse(DougMessages.PartyFull);
            }

            if (_partyRepository.GetPartyByUser(user.Id) != null)
            {
                return new DougResponse(DougMessages.UserHasParty);
            }

            _partyRepository.AddUserToParty(party.Id, user.Id);

            return new DougResponse();
        }

        public DougResponse LeaveParty(User user)
        {
            var party = _partyRepository.GetPartyByUser(user.Id);

            if (party == null)
            {
                return new DougResponse(DougMessages.NoParty);
            }

            _partyRepository.RemoveUserFromParty(party.Id, user.Id);

            return new DougResponse(DougMessages.LeftParty);
        }

        public async Task<DougResponse> SendInvite(User target, User host, string channel)
        {
            var party = _partyRepository.GetPartyByUser(host.Id) ?? _partyRepository.CreateParty(host);

            if (party.Users.Count >= 3)
            {
                return new DougResponse(DougMessages.PartyFullInvite);
            }

            await ShowInvite(party, target, channel);

            return new DougResponse(DougMessages.InviteSent);
        }

        private async Task ShowInvite(Party party, User user, string channel)
        {
            var blocks = new PartyInviteMenu(party, _userService).Blocks;
            await _slack.SendEphemeralBlocks(blocks, user.Id, channel);
        }
    }
}
