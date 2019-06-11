using Doug;
using Doug.Commands;
using Doug.Models;
using Doug.Repositories;
using Doug.Slack;
using Hangfire;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Linq.Expressions;

namespace Test
{
    [TestClass]
    public class GambleChallengeCommandTest
    {
        private const string CommandText = "<@ginette|hea> 10";
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
            _userRepository.Setup(repo => repo.GetUser("ginette")).Returns(new User() {Id = "ginette", Credits = 68});

            _channelRepository.Setup(repo => repo.GetGambleChallenge(User)).Returns(new GambleChallenge("ginette", "testuser", 10));

            _creditsCommands = new CreditsCommands(_userRepository.Object, _slack.Object, _slurRepository.Object, _channelRepository.Object, _backgroundClient.Object);
        }

        [TestMethod]
        public void WhenSendingGambleChallenge_ChallengeIsSaved()
        {
            _creditsCommands.GambleChallenge(command);

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

            var result = _creditsCommands.GambleChallenge(command);

            Assert.AreEqual("You idiot.", result.Message);
        }

        [TestMethod]
        public void WhenSendingGambleChallenge_ChallengeIsBroadcasted()
        {
            _creditsCommands.GambleChallenge(command);

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

            _creditsCommands.GambleChallenge(command);

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

            _creditsCommands.GambleChallenge(command);

            _slack.Verify(slack => slack.SendMessage("<@testuser> need to have at least 10 " + DougMessages.CreditEmoji, Channel));
        }
    }
}
