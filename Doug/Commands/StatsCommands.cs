using System.Threading.Tasks;
using Doug.Menus;
using Doug.Models;
using Doug.Repositories;
using Doug.Slack;

namespace Doug.Commands
{
    public interface IStatsCommands
    {
        DougResponse Balance(Command command);
        Task<DougResponse> Profile(Command command);
        Task<DougResponse> Equipment(Command command);
    }

    public class StatsCommands : IStatsCommands
    {
        private readonly IUserRepository _userRepository;
        private readonly ISlackWebApi _slack;

        private static readonly DougResponse NoResponse = new DougResponse();
        private readonly IPartyRepository _partyRepository;

        public StatsCommands(IUserRepository userRepository, ISlackWebApi messageSender, IPartyRepository partyRepository)
        {
            _userRepository = userRepository;
            _slack = messageSender;
            _partyRepository = partyRepository;
        }
        public DougResponse Balance(Command command)
        {
            var user = _userRepository.GetUser(command.UserId);

            return new DougResponse(string.Format(DougMessages.Balance, user.Credits));
        }

        public async Task<DougResponse> Profile(Command command)
        {
            var userId = command.IsUserArgument() ? command.GetTargetUserId() : command.UserId;
            var user = _userRepository.GetUser(userId);
            var party = _partyRepository.GetPartyByUser(userId);

            if (command.IsUserArgument())
            {
                await _slack.SendEphemeralBlocks(new ShortProfileMenu(user, party).Blocks, command.UserId, command.ChannelId);
                return NoResponse;
            }

            await _slack.SendEphemeralBlocks(new StatsMenu(user).Blocks, command.UserId, command.ChannelId);
            return NoResponse;
        }

        public async Task<DougResponse> Equipment(Command command)
        {
            var user = _userRepository.GetUser(command.UserId);

            await _slack.SendEphemeralBlocks(new EquipmentMenu(user.Loadout).Blocks, command.UserId, command.ChannelId);

            return NoResponse;
        }
    }
}
