using System.Collections.Generic;
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

            return new DougResponse(string.Format(DougMessages.Health, user.Health, user.CalculateTotalHealth()));
        }
        public DougResponse Energy(Command command)
        {
            var user = _userRepository.GetUser(command.UserId);

            return new DougResponse(string.Format(DougMessages.Energy, user.Energy, user.CalculateTotalEnergy()));
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

            var attachments = new List<Attachment>();
            attachments.Add(Attachment.StatsAttachment(slurCount, user));
            attachments.AddRange(Attachment.InventoryAttachments(user));

            _slack.SendAttachments(attachments, command.ChannelId);

            return NoResponse;
        }
    }
}
