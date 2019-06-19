using Doug.Models;
using Doug.Models.Dto;
using Doug.Repositories;
using Doug.Slack;
using System.Collections.Generic;
using System.Linq;

namespace Doug.Commands
{
    public interface ICreditsCommands
    {
        DougResponse Balance(Command command);
        DougResponse Stats(Command command);
        DougResponse Give(Command command);
        DougResponse Forbes(Command command);
        DougResponse Leaderboard(Command command);
    }

    public class CreditsCommands : ICreditsCommands
    {
        private readonly IUserRepository _userRepository;
        private readonly ISlurRepository _slurRepository;
        private readonly ISlackWebApi _slack;

        private static readonly DougResponse NoResponse = new DougResponse();

        public CreditsCommands(IUserRepository userRepository, ISlackWebApi messageSender, ISlurRepository slurRepository)
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

        public DougResponse Give(Command command)
        {
            var amount = int.Parse(command.GetArgumentAt(1));
            var target = command.GetTargetUserId();

            if (amount <= 0)
            {
                return new DougResponse(DougMessages.InvalidAmount);
            }

            var user = _userRepository.GetUser(command.UserId);

            if (!user.HasEnoughCreditsForAmount(amount))
            {
                return user.NotEnoughCreditsForAmountResponse(amount);
            }

            _userRepository.RemoveCredits(command.UserId, amount);
            _userRepository.AddCredits(target, amount);

            var message = string.Format(DougMessages.UserGaveCredits, Utils.UserMention(command.UserId), amount, Utils.UserMention(target));
            _slack.SendMessage(message, command.ChannelId);

            return NoResponse;
        }

        public DougResponse Forbes(Command command)
        {
            var users = _userRepository.GetUsers();

            return new DougResponse(users.Aggregate(string.Empty, (acc, user) => string.Format("{0}{3}{2} = {1}\n", acc, Utils.UserMention(user.Id), user.Credits, DougMessages.CreditEmoji)));
        }

        public DougResponse Stats(Command command)
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

        public DougResponse Leaderboard(Command command)
        {
            var userId = command.UserId;

            if (command.IsUserArgument())
            {
                userId = command.GetTargetUserId();
            }

            var users = _userRepository.GetUsers().ToList();

            users.Sort((u1, u2) => u1.Credits.CompareTo(u2.Credits));
            users.Reverse();
            users = users.GetRange(0, 5);

            List<UsersStatsDto> usersDto = new List<UsersStatsDto>();

            foreach (var user in users)
            {
               usersDto.Add(new UsersStatsDto() { Id = user.Id, Credits = user.Credits, Username = _slack.GetUserInfo(user.Id).Result.Name });
            }

            var attachment = Attachment.LeaderboardAttachment(usersDto);

            _slack.SendAttachment(attachment, command.ChannelId);

            return NoResponse;
        }
    }
}
