using Doug.Commands;
using Doug.Models;
using Doug.Repositories;
using Doug.Slack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace Test
{
    [TestClass]
    public class GiveCommandTest
    {
        private const string CommandText = "<@otherUserid|username> 10";
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

        [TestInitialize]
        public void Setup()
        {
            _userRepository.Setup(repo => repo.GetUser(User)).Returns(new User() { Id = "testuser", Credits = 79});

            _creditsCommands = new CreditsCommands(_userRepository.Object, _slack.Object, _slurRepository.Object);
        }

        [TestMethod]
        public void GivenEnoughCredits_WhenGivingCredits_CreditsAreRemovedFromGiver()
        {
            _creditsCommands.Give(command);

            _userRepository.Verify(repo => repo.RemoveCredits(User, 10));
        }

        [TestMethod]
        public void GivenEnoughCredits_WhenGivingCredits_CreditsAreAddedToReceiver()
        {
            _creditsCommands.Give(command);

            _userRepository.Verify(repo => repo.AddCredits("otherUserid", 10));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GivenNotEnoughCredits_WhenGivingCredits_CreditsAreNotAddedToReceiver()
        {
            _userRepository.Setup(repo => repo.RemoveCredits(User, 10)).Throws(new ArgumentException());
            _creditsCommands.Give(command);

            _userRepository.Verify(repo => repo.AddCredits(User, It.IsAny<int>()), Times.Never);
        }

        [TestMethod]
        public void GivenEnoughCredits_WhenGivingCredits_MessageIsBroadcasted()
        {
            _creditsCommands.Give(command);

            _slack.Verify(slack => slack.SendMessage(It.IsAny<string>(), Channel));
        }
    }
}
