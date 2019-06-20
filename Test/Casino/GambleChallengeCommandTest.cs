using Doug;
using Doug.Commands;
using Doug.Items;
using Doug.Models;
using Doug.Repositories;
using Doug.Services;
using Doug.Slack;
using Hangfire;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Test.Casino
{
    [TestClass]
    public class GambleChallengeCommandTest
    {
        private const string CommandText = "<@ginette|hea> 10";
        private const string Channel = "coco-channel";
        private const string User = "testuser";

        private readonly Command _command = new Command()
        {
            ChannelId = Channel,
            Text = CommandText,
            UserId = User
        };

        private readonly Command _acceptCommand = new Command()
        {
            ChannelId = Channel,
            Text = "accept",
            UserId = User
        };

        private readonly User _caller = new User() { Id = "testuser", Credits = 68 };
        private readonly User _target = new User() {Id = "ginette", Credits = 68};

        private CasinoCommands _casinoCommands;

        private readonly Mock<IUserRepository> _userRepository = new Mock<IUserRepository>();
        private readonly Mock<ISlackWebApi> _slack = new Mock<ISlackWebApi>();
        private readonly Mock<IChannelRepository> _channelRepository = new Mock<IChannelRepository>();
        private readonly Mock<IBackgroundJobClient> _backgroundClient = new Mock<IBackgroundJobClient>();
        private readonly Mock<IItemEventDispatcher> _itemEventDispatcher = new Mock<IItemEventDispatcher>();
        private readonly Mock<IStatsRepository> _statsRepository = new Mock<IStatsRepository>();
        private readonly Mock<IRandomService> _randomService = new Mock<IRandomService>();

        [TestInitialize]
        public void Setup()
        {
            _userRepository.Setup(repo => repo.GetUser(User)).Returns(_caller);
            _userRepository.Setup(repo => repo.GetUser("ginette")).Returns(_target);

            _channelRepository.Setup(repo => repo.GetGambleChallenge(User)).Returns(new GambleChallenge("testuser", "ginette", 10));

            _casinoCommands = new CasinoCommands(_userRepository.Object, _slack.Object, _channelRepository.Object, _backgroundClient.Object, _itemEventDispatcher.Object, _statsRepository.Object, _randomService.Object);
        }

        [TestMethod]
        public void GivenCallerWins_WhenGambling_CallerGainHisBet()
        {
            _randomService.Setup(rnd => rnd.RollAgainstOpponent(It.IsAny<double>(), It.IsAny<double>())).Returns(true);

            _casinoCommands.GambleChallenge(_acceptCommand);

            _userRepository.Verify(repo => repo.AddCredits(User, 10));
        }

        [TestMethod]
        public void GivenCallerLose_WhenGambling_CallerLoseHisBet()
        {
            _randomService.Setup(rnd => rnd.RollAgainstOpponent(It.IsAny<double>(), It.IsAny<double>())).Returns(false);

            _casinoCommands.GambleChallenge(_acceptCommand);

            _userRepository.Verify(repo => repo.RemoveCredits(User, 10));
        }

        [TestMethod]
        public void GivenTargetWins_WhenGambling_TargetGainHisBet()
        {
            _randomService.Setup(rnd => rnd.RollAgainstOpponent(It.IsAny<double>(), It.IsAny<double>())).Returns(false);

            _casinoCommands.GambleChallenge(_acceptCommand);

            _userRepository.Verify(repo => repo.AddCredits("ginette", 10));
        }

        [TestMethod]
        public void GivenTargetLose_WhenGambling_TargetLoseHisBet()
        {
            _randomService.Setup(rnd => rnd.RollAgainstOpponent(It.IsAny<double>(), It.IsAny<double>())).Returns(true);

            _casinoCommands.GambleChallenge(_acceptCommand);

            _userRepository.Verify(repo => repo.RemoveCredits("ginette", 10));
        }

        [TestMethod]
        public void WhenSendingGambleChallenge_ChallengeIsSaved()
        {
            _casinoCommands.GambleChallenge(_command);

            _channelRepository.Verify(repo => repo.SendGambleChallenge(It.IsAny<GambleChallenge>()));
        }

        [TestMethod]
        public void GivenNegativeAmount_WhenSendingGambleChallenge_ErrorMessageIsSent()
        {
            var command = new Command()
            {
                ChannelId = Channel,
                Text = "<@ginette|hea> -10",
                UserId = User
            };

            var result = _casinoCommands.GambleChallenge(command);

            Assert.AreEqual("You idiot.", result.Message);
        }

        [TestMethod]
        public void WhenSendingGambleChallenge_ChallengeIsBroadcasted()
        {
            _casinoCommands.GambleChallenge(_command);

            _slack.Verify(slack => slack.SendMessage(It.IsAny<string>(), Channel));
        }

        [TestMethod]
        public void GivenRequesterIsBroke_WhenAcceptingGambleChallenge_ChallengeIsCancelled()
        {
            _userRepository.Setup(repo => repo.GetUser("ginette")).Returns(new User() { Id = "ginette", Credits = 7 });

            var command = new Command()
            {
                ChannelId = Channel,
                Text = "accept",
                UserId = User
            };

            _casinoCommands.GambleChallenge(command);

            _slack.Verify(slack => slack.SendMessage("<@ginette> need to have at least 10 " + DougMessages.CreditEmoji, Channel));
        }

        [TestMethod]
        public void GivenTargetIsBroke_WhenAcceptingGambleChallenge_ChallengeIsCancelled()
        {
            _userRepository.Setup(repo => repo.GetUser("testuser")).Returns(new User() { Id = "testuser", Credits = 7 });

            var command = new Command()
            {
                ChannelId = Channel,
                Text = "accept",
                UserId = User
            };

            _casinoCommands.GambleChallenge(command);

            _slack.Verify(slack => slack.SendMessage("<@testuser> need to have at least 10 " + DougMessages.CreditEmoji, Channel));
        }

        [TestMethod]
        public void WhenGambling_GetCallerChanceFromItems()
        {
            var command = new Command()
            {
                ChannelId = Channel,
                Text = "accept",
                UserId = User
            };

            _casinoCommands.GambleChallenge(command);

            _itemEventDispatcher.Verify(dispatcher => dispatcher.OnGambling(_caller, It.IsAny<double>()));
        }

        [TestMethod]
        public void WhenGambling_GetTargetChanceFromItems()
        {
            var command = new Command()
            {
                ChannelId = Channel,
                Text = "accept",
                UserId = User
            };

            _casinoCommands.GambleChallenge(command);

            _itemEventDispatcher.Verify(dispatcher => dispatcher.OnGambling(_target, It.IsAny<double>()));
        }
    }
}
