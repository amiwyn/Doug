using Doug;
using Doug.Commands;
using Doug.Items;
using Doug.Models;
using Doug.Repositories;
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

        private readonly User _user = new User() {Id = "testuser", Credits = 68};

        private CasinoCommands _casinoCommands;

        private readonly Mock<IUserRepository> _userRepository = new Mock<IUserRepository>();
        private readonly Mock<ISlackWebApi> _slack = new Mock<ISlackWebApi>();
        private readonly Mock<IChannelRepository> _channelRepository = new Mock<IChannelRepository>();
        private readonly Mock<IBackgroundJobClient> _backgroundClient = new Mock<IBackgroundJobClient>();
        private readonly Mock<IItemEventDispatcher> _itemEventDispatcher = new Mock<IItemEventDispatcher>();


        [TestInitialize]
        public void Setup()
        {
            _userRepository.Setup(repo => repo.GetUser(User)).Returns(_user);

            _casinoCommands = new CasinoCommands(_userRepository.Object, _slack.Object, _channelRepository.Object, _backgroundClient.Object, _itemEventDispatcher.Object);
        }

        [TestMethod]
        public void GivenUserHasEnoughCredits_WhenGambling_UserCanGamble()
        {
            _casinoCommands.Gamble(_command);

            _slack.Verify(slack => slack.SendMessage(It.IsAny<string>(), Channel));
        }

        [TestMethod]
        public void GivenUserHasNotEnoughCredits_WhenGambling_UserReceiveNotEnoughCreditsMessage()
        {
            _userRepository.Setup(repo => repo.GetUser(User)).Returns(new User() { Id = "testuser", Credits = 9 });

            var result = _casinoCommands.Gamble(_command);

            Assert.AreEqual("You need 10 " + DougMessages.CreditEmoji + " to do this and you have 9 " + DougMessages.CreditEmoji, result.Message);
        }

        [TestMethod]
        public void GivenNesgativeAmount_WhenGambling_UserReceiveInvalidAmountMessage()
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
        public void GivenUserIsTooRich_WhenGambling_UserReceiveUserTooRichMessage()
        {
            _userRepository.Setup(repo => repo.GetUser(User)).Returns(new User() { Id = "testuser", Credits = 368 });

            var result = _casinoCommands.Gamble(_command);

            Assert.AreEqual("You are too rich for this.", result.Message);
        }

        [TestMethod]
        public void GivenUserIsRichEnough_WhenGambling_UserCanGamble()
        {
            _userRepository.Setup(repo => repo.GetUser(User)).Returns(new User() { Id = "testuser", Credits = 268 });

            _casinoCommands.Gamble(_command);

            _slack.Verify(slack => slack.SendMessage(It.IsAny<string>(), Channel));
        }

        [TestMethod]
        public void WhenGambling_GetUserChanceFromItems()
        {
            _casinoCommands.Gamble(_command);

            _itemEventDispatcher.Verify(dispatcher => dispatcher.OnGambling(_user));
        }
    }
}
