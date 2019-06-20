using Doug.Models;
using Doug.Repositories;
using Doug.Slack;
using System.Linq;

namespace Doug.Commands
{
    public interface IStatsCommands
    {
        DougResponse Balance(Command command);
        DougResponse Health(Command command);
        DougResponse Energy(Command command);
        DougResponse Profile(Command command);
    }

    public class StatsCommands : IStatsCommands
    {
        private readonly IUserRepository _userRepository;
        private readonly ISlurRepository _slurRepository;
        private readonly ISlackWebApi _slack;

        private static readonly DougResponse NoResponse = new DougResponse();

        public StatsCommands(IUserRepository userRepository, ISlackWebApi messageSender, ISlurRepository slurRepository)
        {
            _userRepository = userRepository;
            _slack = messageSender;
            _slurRepository = slurRepository;
        }
        public DougResponse Balance(Command command)
        {
            var user = _userRepository.GetUser(command.UserId);

            return new DougResponse(string.Format(DougMessages.Balance, user.Credits));
        }
        public DougResponse Health(Command command)
        {
            var user = _userRepository.GetUser(command.UserId);

            return new DougResponse(string.Format(DougMessages.Health, user.Health, 100)); // TODO: Add prop MaxEnergy to user, and display here.
        }
        public DougResponse Energy(Command command)
        {
            var user = _userRepository.GetUser(command.UserId);

            return new DougResponse(string.Format(DougMessages.Energy, user.Energy, 25)); // TODO: Add prop MaxEnergy to user, and display here.
        }

        public DougResponse Profile(Command command)
        {
            var userId = command.UserId;

            if (command.IsUserArgument())
            {
                userId = command.GetTargetUserId();
            }

            var slurCount = _slurRepository.GetSlursFrom(userId).Count();
            var user = _userRepository.GetUser(userId);

            var attachment = Attachment.StatsAttachment(slurCount, user);

            _slack.SendAttachment(attachment, command.ChannelId);

            return NoResponse;
        }
    }
}
