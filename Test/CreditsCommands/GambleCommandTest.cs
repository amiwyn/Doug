using Doug.Commands;
using Doug.Models;
using Doug.Repositories;
using Doug.Slack;
using Hangfire;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using Doug;

namespace Test
{
    [TestClass]
    public class GambleCommandTest
    {
        private const string CommandText = "10";
        private const string Channel = "coco-channel";
        private const string User = "testuser";

        private readonly Command command = new Command()
        {
            ChannelId = Channel,
            Text = CommandText,
            UserId = User
        };

        private CreditsCommands _creditsCommands;

        private readonly Mock<IUserRepository> _userRepository = new Mock<IUserRepository>();
        private readonly Mock<ISlurRepository> _slurRepository = new Mock<ISlurRepository>();
        private readonly Mock<ISlackWebApi> _slack = new Mock<ISlackWebApi>();
        private readonly Mock<IChannelRepository> _channelRepository = new Mock<IChannelRepository>();
        private readonly Mock<IBackgroundJobClient> _backgroundClient = new Mock<IBackgroundJobClient>();

        [TestInitialize]
        public void Setup()
        {
            _userRepository.Setup(repo => repo.GetUser(User)).Returns(new User() { Id = "testuser", Credits = 68});

            _creditsCommands = new CreditsCommands(_userRepository.Object, _slack.Object, _slurRepository.Object, _channelRepository.Object, _backgroundClient.Object);
        }

        [TestMethod]
        public void GivenUserHasEnoughCredits_WhenGambling_UserCanGamble()
        {
            _creditsCommands.Gamble(command);

            _slack.Verify(slack => slack.SendMessage(It.IsAny<string>(), Channel));
        }

        [TestMethod]
        public void GivenUserHasNotEnoughCredits_WhenGambling_UserReceiveNotEnoughCreditsMessage()
        {
            _userRepository.Setup(repo => repo.GetUser(User)).Returns(new User() { Id = "testuser", Credits = 9 });

            var result = _creditsCommands.Gamble(command);

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

            var result = _creditsCommands.Gamble(command);

            Assert.AreEqual("Invalid amount", result.Message);
        }

        [TestMethod]
        public void GivenUserIsTooRich_WhenGambling_UserReceiveUserTooRichMessage()
        {
            _userRepository.Setup(repo => repo.GetUser(User)).Returns(new User() { Id = "testuser", Credits = 368 });

            var result = _creditsCommands.Gamble(command);

            Assert.AreEqual("You are too rich for this.", result.Message);
        }

        [TestMethod]
        public void GivenUserIsRichEnough_WhenGambling_UserCanGamble()
        {
            _userRepository.Setup(repo => repo.GetUser(User)).Returns(new User() { Id = "testuser", Credits = 268 });

            var result = _creditsCommands.Gamble(command);

            _slack.Verify(slack => slack.SendMessage(It.IsAny<string>(), Channel));
        }
    }
}
