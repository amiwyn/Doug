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
        DougResponse Equipment(Command command);
    }

    public class StatsCommands : IStatsCommands
    {
        private readonly IUserRepository _userRepository;
        private readonly ISlackWebApi _slack;

        private static readonly DougResponse NoResponse = new DougResponse();

        public StatsCommands(IUserRepository userRepository, ISlackWebApi messageSender)
        {
            _userRepository = userRepository;
            _slack = messageSender;
        }
        public DougResponse Balance(Command command)
        {
            var user = _userRepository.GetUser(command.UserId);

            return new DougResponse(string.Format(DougMessages.Balance, user.Credits));
        }

        public async Task<DougResponse> Profile(Command command)
        {
            var user = _userRepository.GetUser(command.UserId);

            await _slack.SendEphemeralBlocks(new StatsMenu(user).Blocks, command.UserId, command.ChannelId);

            return NoResponse;
        }

        public DougResponse Equipment(Command command)
        {
            var userId = command.UserId;

            if (command.IsUserArgument())
            {
                userId = command.GetTargetUserId();
            }

            var user = _userRepository.GetUser(userId);

            var attachments = Attachment.EquipmentAttachments(user.Loadout);

            _slack.SendAttachments(attachments, command.ChannelId);

            return NoResponse;
        }
    }
}
