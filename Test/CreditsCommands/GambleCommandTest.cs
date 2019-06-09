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

        [TestInitialize]
        public void Setup()
        {
            _userRepository.Setup(repo => repo.GetUser(User)).Returns(new User() { Id = "testuser", Credits = 68});

            _creditsCommands = new CreditsCommands(_userRepository.Object, _slack.Object, _slurRepository.Object);
        }

        [TestMethod]
        public void GivenLessThan200Credits_WhenGambling_UserCanGamble()
        {
            _creditsCommands.Gamble(command);

            _userRepository.Verify(repo => repo.RemoveCredits(User, 10));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GivenMoreThan200Credits_WhenGambling_ExceptionIsThrown()
        {
            _userRepository.Setup(repo => repo.GetUser(User)).Returns(new User() { Id = "testuser", Credits = 300 });

            _creditsCommands.Gamble(command);
        }
    }
}
