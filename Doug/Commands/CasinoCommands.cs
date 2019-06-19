using Doug.Models;
using Doug.Repositories;
using Doug.Slack;
using Hangfire;
using System;
using Doug.Items;
using System.Linq;

namespace Doug.Commands
{
    public interface ICasinoCommands
    {
        DougResponse Gamble(Command command);
        DougResponse GambleChallenge(Command command);
    }

    public class CasinoCommands : ICasinoCommands
    {
        private const int MinimumGambleAmount = 10;
        private const int GambleCreditLimit = 300;
        private const string AcceptChallengeWord = "accept";
        private const string DeclineChallengeWord = "decline";
        private readonly IUserRepository _userRepository;
        private readonly IChannelRepository _channelRepository;
        private readonly ISlackWebApi _slack;
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly IItemEventDispatcher _itemEventDispatcher;


        private static readonly DougResponse NoResponse = new DougResponse();

        public CasinoCommands(IUserRepository userRepository, ISlackWebApi messageSender, IChannelRepository channelRepository, IBackgroundJobClient backgroundJobClient, IItemEventDispatcher itemEventDispatcher)
        {
            _userRepository = userRepository;
            _slack = messageSender;
            _channelRepository = channelRepository;
            _backgroundJobClient = backgroundJobClient;
            _itemEventDispatcher = itemEventDispatcher;
        }

        public DougResponse Gamble(Command command)
        {
            var user = _userRepository.GetUser(command.UserId);
            var amount = int.Parse(command.GetArgumentAt(0));

            if (amount < 0)
            {
                return new DougResponse(DougMessages.InvalidAmount);
            }

            if (user.Credits > GambleCreditLimit)
            {
                return new DougResponse(DougMessages.YouAreTooRich);
            }

            if (!user.HasEnoughCreditsForAmount(amount))
            {
                return user.NotEnoughCreditsForAmountResponse(amount);
            }

            string baseMessage;
            if (UserCoinFlipWin(user))
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

            if (amount > MinimumGambleAmount)
            {
                _slack.SendMessage(message, command.ChannelId);
            }
            else
            {
                _slack.SendEphemeralMessage(message, command.UserId, command.ChannelId);
            }

            return NoResponse;
        }

        private bool UserCoinFlipWin(User user)
        {
            var random = new Random();
            var flipResult = random.NextDouble();
            var userChance = _itemEventDispatcher.OnGambling(user, user.CalculateBaseGambleChance());
            return flipResult < userChance;
        }

        public DougResponse GambleChallenge(Command command)
        {
            if (command.IsUserArgument())
            {
                return SendChallenge(command);
            }

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

        private DougResponse SendChallenge(Command command)
        {
            var amount = int.Parse(command.GetArgumentAt(1));
            var targetId = command.GetTargetUserId();

            if (amount <= 0 || command.UserId == targetId)
            {
                return new DougResponse(DougMessages.YouIdiot);
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

            var winner = target;
            var loser = requester;

            if (VersusCoinFlipWin(requester, target))
            {
                winner = requester;
                loser = target;
            }

            _userRepository.RemoveCredits(loser.Id, challenge.Amount);
            _userRepository.AddCredits(winner.Id, challenge.Amount);

            _channelRepository.RemoveGambleChallenge(challenge.TargetId);

            var message = string.Format(DougMessages.GambleChallenge, Utils.UserMention(winner.Id), challenge.Amount, Utils.UserMention(loser.Id));
            _slack.SendMessage(message, command.ChannelId);
        }

        private bool VersusCoinFlipWin(User caller, User target)
        {
            var random = new Random();
            var flipResult = random.NextDouble();
            var callerChance = _itemEventDispatcher.OnGambling(caller, caller.CalculateBaseGambleChance());
            var targetChance = _itemEventDispatcher.OnGambling(target, target.CalculateBaseGambleChance());
            var winChance = 0.5 + callerChance - targetChance;
            return flipResult < winChance;
        }
    }
}
