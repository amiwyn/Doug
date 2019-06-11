using Doug.Models;
using Doug.Repositories;
using Doug.Slack;
using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Doug.Commands
{
    public interface ICreditsCommands
    {
        DougResponse Balance(Command command);
        DougResponse Stats(Command command);
        DougResponse Give(Command command);
        DougResponse Gamble(Command command);
        DougResponse GambleChallenge(Command command);
    }

    public class CreditsCommands : ICreditsCommands
    {
        private const int GambleCreditLimit = 200;
        private const string AcceptChallengeWord = "accept";
        private const string DeclineChallengeWord = "decline";
        private readonly IUserRepository _userRepository;
        private readonly ISlurRepository _slurRepository;
        private readonly IChannelRepository _channelRepository;
        private readonly ISlackWebApi _slack;
        private readonly IBackgroundJobClient _backgroundJobClient;

        private static readonly DougResponse NoResponse = new DougResponse();

        public CreditsCommands(IUserRepository userRepository, ISlackWebApi messageSender, ISlurRepository slurRepository, IChannelRepository channelRepository, IBackgroundJobClient backgroundJobClient)
        {
            _userRepository = userRepository;
            _slack = messageSender;
            _slurRepository = slurRepository;
            _channelRepository = channelRepository;
            _backgroundJobClient = backgroundJobClient;
        }
        public DougResponse Balance(Command command)
        {
            var user = _userRepository.GetUser(command.UserId);

            return new DougResponse(string.Format(DougMessages.Balance, user.Credits));
        }

        public DougResponse Gamble(Command command)
        {
            var user = _userRepository.GetUser(command.UserId);
            var amount = int.Parse(command.GetArgumentAt(0));

            if (user.Credits > GambleCreditLimit)
            {
                return new DougResponse(DougMessages.YouAreTooRich);
            }

            if (!user.HasEnoughCreditsForAmount(amount))
            {
                return user.NotEnoughCreditsForAmountResponse(amount);
            }

            string baseMessage;
            if (CoinflipWin())
            {
                baseMessage = DougMessages.WonGamble;
                _userRepository.AddCredits(command.UserId, amount);
            }
            else
            {
                baseMessage = DougMessages.LostGamble;
                _userRepository.RemoveCredits(command.UserId, amount);
            }

            var message = string.Format(baseMessage, Utils.UserMention(command.UserId), amount);
            _slack.SendMessage(message, command.ChannelId);

            return NoResponse;
        }

        private bool CoinflipWin()
        {
            var random = new Random();
            return random.Next(2) != 0;
        }

        public DougResponse Give(Command command)
        {
            var amount = int.Parse(command.GetArgumentAt(1));
            var target = command.GetTargetUserId();

            if (amount < 0)
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

        public DougResponse GambleChallenge(Command command)
        {
            if (command.IsUserArgument())
            {
                return SendChallenge(command);
            }
            else
            {
                if (!IsUserChallenged(command.UserId))
                {
                    return new DougResponse(DougMessages.NotChallenged);
                }

                if (command.GetArgumentAt(0).ToLower() == AcceptChallengeWord)
                {
                    GambleVersus(command);
                }

                if (command.GetArgumentAt(0).ToLower() == DeclineChallengeWord)
                {
                    var challenge = _channelRepository.GetGambleChallenge(command.UserId);
                    _slack.SendMessage(string.Format(DougMessages.GambleDeclined, Utils.UserMention(command.UserId), Utils.UserMention(challenge.RequesterId)), command.ChannelId);
                    _channelRepository.RemoveGambleChallenge(challenge.TargetId);
                }

                return NoResponse;
            }
        }

        private DougResponse SendChallenge(Command command)
        {
            int amount = int.Parse(command.GetArgumentAt(1));
            var targetId = command.GetTargetUserId();

            if (amount < 0 || command.UserId == targetId)
            {
                return new DougResponse("You idiot.");
            }

            var challenge = _channelRepository.GetGambleChallenge(targetId);

            if (challenge != null)
            {
                return new DougResponse(DougMessages.AlreadyChallenged);
            }

            _channelRepository.SendGambleChallenge(new GambleChallenge(command.UserId, targetId, amount));

            _backgroundJobClient.Schedule(() => ChallengeTimeout(targetId), TimeSpan.FromMinutes(3));

            _slack.SendMessage(string.Format(DougMessages.ChallengeSent, Utils.UserMention(command.UserId), Utils.UserMention(targetId), amount), command.ChannelId);
            _slack.SendEphemeralMessage(DougMessages.GambleChallengeTip, targetId, command.ChannelId);

            return NoResponse;
        }

        public void ChallengeTimeout(string target)
        {
            _channelRepository.RemoveGambleChallenge(target);
        }

        private bool IsUserChallenged(string userId)
        {
            var challenge = _channelRepository.GetGambleChallenge(userId);
            return challenge != null;
        }

        private void GambleVersus(Command command)
        {
            var challenge = _channelRepository.GetGambleChallenge(command.UserId);
            var requester = _userRepository.GetUser(challenge.RequesterId);
            var target = _userRepository.GetUser(challenge.TargetId);

            if (!requester.HasEnoughCreditsForAmount(challenge.Amount))
            {
                _slack.SendMessage(string.Format(DougMessages.InsufficientCredits, Utils.UserMention(requester.Id), challenge.Amount), command.ChannelId);
                return;
            }

            if (!target.HasEnoughCreditsForAmount(challenge.Amount))
            {
                _slack.SendMessage(string.Format(DougMessages.InsufficientCredits, Utils.UserMention(target.Id), challenge.Amount), command.ChannelId);
                return;
            }

            var winner = requester;
            var loser = target;

            if (CoinflipWin())
            {
                winner = target;
                loser = requester;
            }

            _userRepository.RemoveCredits(loser.Id, challenge.Amount);
            _userRepository.AddCredits(winner.Id, challenge.Amount);

            _channelRepository.RemoveGambleChallenge(challenge.TargetId);

            var message = string.Format(DougMessages.GambleChallenge, Utils.UserMention(winner.Id), challenge.Amount, Utils.UserMention(loser.Id));
            _slack.SendMessage(message, command.ChannelId);
        }
    }
}
