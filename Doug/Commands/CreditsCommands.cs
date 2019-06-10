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
        string Balance(Command command);
        void Stats(Command command);
        void Give(Command command);
        void Gamble(Command command);
        void GambleChallenge(Command command);
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

        public CreditsCommands(IUserRepository userRepository, ISlackWebApi messageSender, ISlurRepository slurRepository, IChannelRepository channelRepository, IBackgroundJobClient backgroundJobClient)
        {
            _userRepository = userRepository;
            _slack = messageSender;
            _slurRepository = slurRepository;
            _channelRepository = channelRepository;
            _backgroundJobClient = backgroundJobClient;
        }
        public string Balance(Command command)
        {
            var user = _userRepository.GetUser(command.UserId);

            return string.Format(DougMessages.Balance, user.Credits);
        }

        public void Gamble(Command command)
        {
            var user = _userRepository.GetUser(command.UserId);
            var amount = int.Parse(command.GetArgumentAt(0));

            if (user.Credits > GambleCreditLimit)
            {
                throw new UserTooRichException();
            }

            _userRepository.RemoveCredits(command.UserId, amount);
            string baseMessage = DougMessages.LostGamble;

            if (CoinflipWin())
            {
                baseMessage = DougMessages.WonGamble;
                _userRepository.AddCredits(command.UserId, amount*2);
            }

            var message = string.Format(baseMessage, Utils.UserMention(command.UserId), amount);
            _slack.SendMessage(message, command.ChannelId);
        }

        private bool CoinflipWin()
        {
            var random = new Random();
            return random.Next(2) != 0;
        }

        public void Give(Command command)
        {
            var amount = int.Parse(command.GetArgumentAt(1));
            var target = command.GetTargetUserId();

            _userRepository.RemoveCredits(command.UserId, amount);
            _userRepository.AddCredits(target, amount);

            var message = string.Format(DougMessages.UserGaveCredits, Utils.UserMention(command.UserId), amount, Utils.UserMention(target));
            _slack.SendMessage(message, command.ChannelId);
        }

        public void Stats(Command command)
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
        }

        public void GambleChallenge(Command command)
        {
            if (command.IsUserArgument())
            {
                SendChallenge(command);
            }
            else
            {
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
            }
        }

        private void SendChallenge(Command command)
        {
            int amount = int.Parse(command.GetArgumentAt(1));
            var targetId = command.GetTargetUserId();

            if (amount < 0)
            {
                throw new ArgumentException("You idiot.");
            }

            _channelRepository.SendGambleChallenge(new GambleChallenge(command.UserId, targetId, amount));

            _backgroundJobClient.Schedule(() => ChallengeTimeout(targetId), TimeSpan.FromMinutes(3));

            _slack.SendMessage(string.Format(DougMessages.ChallengeSent, Utils.UserMention(command.UserId), Utils.UserMention(targetId), amount), command.ChannelId);
            _slack.SendEphemeralMessage(DougMessages.GambleChallengeTip, targetId, command.ChannelId);
        }

        public void ChallengeTimeout(string target)
        {
            _channelRepository.RemoveGambleChallenge(target);
        }

        private void GambleVersus(Command command)
        {
            var challenge = _channelRepository.GetGambleChallenge(command.UserId);
            var requester = _userRepository.GetUser(challenge.RequesterId);
            var target = _userRepository.GetUser(challenge.TargetId);

            if (requester.Credits < challenge.Amount)
            {
                _slack.SendMessage(string.Format(DougMessages.InsufficientCredits, Utils.UserMention(requester.Id), challenge.Amount), command.ChannelId);
                return;
            }

            if (target.Credits < challenge.Amount)
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
