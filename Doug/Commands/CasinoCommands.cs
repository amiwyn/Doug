using Doug.Models;
using Doug.Repositories;
using Doug.Slack;
using Hangfire;
using System;
using Doug.Items;
using Doug.Services;

namespace Doug.Commands
{
    public interface ICasinoCommands
    {
        DougResponse Gamble(Command command);
        DougResponse GambleChallenge(Command command);
    }

    public class CasinoCommands : ICasinoCommands
    {
        private const string AcceptChallengeWord = "accept";
        private const string DeclineChallengeWord = "decline";
        private readonly IUserRepository _userRepository;
        private readonly IChannelRepository _channelRepository;
        private readonly ISlackWebApi _slack;
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly IEventDispatcher _eventDispatcher;
        private readonly IRandomService _randomService;


        private static readonly DougResponse NoResponse = new DougResponse();
        private readonly IStatsRepository _statsRepository;
        private readonly IUserService _userService;

        public CasinoCommands(IUserRepository userRepository, ISlackWebApi messageSender, IChannelRepository channelRepository, IBackgroundJobClient backgroundJobClient, IEventDispatcher eventDispatcher, IStatsRepository statsRepository, IRandomService randomService, IUserService userService)
        {
            _userRepository = userRepository;
            _slack = messageSender;
            _channelRepository = channelRepository;
            _backgroundJobClient = backgroundJobClient;
            _eventDispatcher = eventDispatcher;
            _statsRepository = statsRepository;
            _randomService = randomService;
            _userService = userService;
        }

        public DougResponse Gamble(Command command)
        {
            var user = _userRepository.GetUser(command.UserId);
            var amount = int.Parse(command.GetArgumentAt(0));

            if (amount < 0)
            {
                return new DougResponse(DougMessages.InvalidAmount);
            }

            if (!user.HasEnoughCreditsForAmount(amount))
            {
                return new DougResponse(user.NotEnoughCreditsForAmountResponse(amount));
            }

            var cost = (int)Math.Ceiling(amount / 10.0);
            var energy = user.Energy - cost;

            if (energy < 0)
            {
                return new DougResponse(DougMessages.NotEnoughEnergy);
            }

            _statsRepository.UpdateEnergy(command.UserId, energy);

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

            var message = string.Format(baseMessage, _userService.Mention(user), amount);
            _slack.BroadcastMessage(message, command.ChannelId);

            return NoResponse;
        }

        private bool UserCoinFlipWin(User user)
        {
            var userChance = _eventDispatcher.OnGambling(user, user.BaseGambleChance());
            return _randomService.RollAgainstOpponent(userChance, 0.5);
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
                var target = _userRepository.GetUser(command.UserId);
                var challenger = _userRepository.GetUser(challenge.RequesterId);

                _slack.BroadcastMessage(string.Format(DougMessages.GambleDeclined, _userService.Mention(target), _userService.Mention(challenger)), command.ChannelId);
                _channelRepository.RemoveGambleChallenge(challenge.TargetId);
            }

            return NoResponse;
        }

        private DougResponse SendChallenge(Command command)
        {
            var amount = int.Parse(command.GetArgumentAt(1));
            var targetId = command.GetTargetUserId();
            var caller = _userRepository.GetUser(command.UserId);
            var target = _userRepository.GetUser(targetId);

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

            _slack.BroadcastMessage(string.Format(DougMessages.ChallengeSent, _userService.Mention(caller), _userService.Mention(target), amount), command.ChannelId);
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
                _slack.BroadcastMessage(string.Format(DougMessages.InsufficientCredits, _userService.Mention(requester), challenge.Amount), command.ChannelId);
                return;
            }

            if (!target.HasEnoughCreditsForAmount(challenge.Amount))
            {
                _slack.BroadcastMessage(string.Format(DougMessages.InsufficientCredits, _userService.Mention(target), challenge.Amount), command.ChannelId);
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

            var message = string.Format(DougMessages.GambleChallenge, _userService.Mention(winner), challenge.Amount, _userService.Mention(loser));
            _slack.BroadcastMessage(message, command.ChannelId);
        }

        private bool VersusCoinFlipWin(User caller, User target)
        {
            var callerChance = _eventDispatcher.OnGambling(caller, caller.BaseGambleChance());
            var targetChance = _eventDispatcher.OnGambling(target, target.BaseGambleChance());

            return _randomService.RollAgainstOpponent(callerChance, targetChance);
        }
    }
}
