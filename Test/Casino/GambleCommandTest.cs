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
    public class GambleCommandTest
    {
        private const string CommandText = "10";
        private const string Channel = "coco-channel";
        private const string User = "testuser";

        private readonly Command _command = new Command()
        {
            ChannelId = Channel,
            Text = CommandText,
            UserId = User
        };

        private readonly User _user = new User {Id = "testuser", Credits = 68, Energy = 20};

        private CasinoCommands _casinoCommands;

        private readonly Mock<IUserRepository> _userRepository = new Mock<IUserRepository>();
        private readonly Mock<ISlackWebApi> _slack = new Mock<ISlackWebApi>();
        private readonly Mock<IChannelRepository> _channelRepository = new Mock<IChannelRepository>();
        private readonly Mock<IBackgroundJobClient> _backgroundClient = new Mock<IBackgroundJobClient>();
        private readonly Mock<IEventDispatcher> _itemEventDispatcher = new Mock<IEventDispatcher>();
        private readonly Mock<IStatsRepository> _statsRepository = new Mock<IStatsRepository>();
        private readonly Mock<IRandomService> _randomService = new Mock<IRandomService>();
        private readonly Mock<IUserService> _userService = new Mock<IUserService>();
        private readonly Mock<ICreditsRepository> _creditsRepository = new Mock<ICreditsRepository>();

        [TestInitialize]
        public void Setup()
        {
            _userRepository.Setup(repo => repo.GetUser(User)).Returns(_user);

            _casinoCommands = new CasinoCommands(_userRepository.Object, _slack.Object, _channelRepository.Object, _backgroundClient.Object, _itemEventDispatcher.Object, _statsRepository.Object, _randomService.Object, _userService.Object, _creditsRepository.Object);
        }

        [TestMethod]
        public void GivenUserWins_WhenGambling_UserGainHisBet()
        {
            _randomService.Setup(rnd => rnd.RollAgainstOpponent(It.IsAny<double>(), It.IsAny<double>())).Returns(true);

            _casinoCommands.Gamble(_command);

            _creditsRepository.Verify(repo => repo.AddCredits(User, 10));
        }

        [TestMethod]
        public void GivenUserLose_WhenGambling_UserLoseHisBet()
        {
            _randomService.Setup(rnd => rnd.RollAgainstOpponent(It.IsAny<double>(), It.IsAny<double>())).Returns(false);

            _casinoCommands.Gamble(_command);

            _creditsRepository.Verify(repo => repo.RemoveCredits(User, 10));
        }

        [TestMethod]
        public void GivenUserHasEnoughCredits_WhenGambling_UserCanGamble()
        {
            _casinoCommands.Gamble(_command);

            _slack.Verify(slack => slack.BroadcastMessage(It.IsAny<string>(), Channel));
        }

        [TestMethod]
        public void GivenUserHasNotEnoughCredits_WhenGambling_UserReceiveNotEnoughCreditsMessage()
        {
            _userRepository.Setup(repo => repo.GetUser(User)).Returns(new User() { Id = "testuser", Credits = 9 });

            var result = _casinoCommands.Gamble(_command);

            Assert.AreEqual("You need 10 " + DougMessages.CreditEmoji + " to do this and you have 9 " + DougMessages.CreditEmoji, result.Message);
        }

        [TestMethod]
        public void GivenNegativeAmount_WhenGambling_UserReceiveInvalidAmountMessage()
        {
            var command = new Command()
            {
                ChannelId = Channel,
                Text = "-153",
                UserId = User
            };

            var result = _casinoCommands.Gamble(command);

            Assert.AreEqual("Invalid amount", result.Message);
        }

        [TestMethod]
        public void WhenGambling_UserLoseTenPercentOfAmountInEnergy()
        {
            _casinoCommands.Gamble(_command);

            _statsRepository.Verify(repo => repo.UpdateEnergy(User, 19));
        }

        [TestMethod]
        public void GivenUserHasNotEnoughEnergy_WhenGambling_UserReceiveNotEnoughEnergyMessage()
        {
            _userRepository.Setup(repo => repo.GetUser(User)).Returns(new User { Id = "testuser", Credits = 68, Energy = 0 });

            var result = _casinoCommands.Gamble(_command);

            Assert.AreEqual(DougMessages.NotEnoughEnergy, result.Message);
        }

        [TestMethod]
        public void GivenUserIsRich_WhenGambling_UserCanGamble()
        {
            _userRepository.Setup(repo => repo.GetUser(User)).Returns(new User() { Id = "testuser", Credits = 8912, Energy = 33});

            _casinoCommands.Gamble(_command);

            _slack.Verify(slack => slack.BroadcastMessage(It.IsAny<string>(), Channel));
        }

        [TestMethod]
        public void WhenGambling_GetUserChanceFromItems()
        {
            _casinoCommands.Gamble(_command);

            _itemEventDispatcher.Verify(dispatcher => dispatcher.OnGambling(_user, It.IsAny<double>()));
        }
    }
}
