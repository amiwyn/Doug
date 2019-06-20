using System.Collections.Generic;
using Doug.Commands;
using Doug.Models;
using Doug.Repositories;
using Doug.Slack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Test.Credits
{
    [TestClass]
    public class LeaderboardCommands
    {
        private const string Channel = "test";
        private const string User = "test";

        private CreditsCommands _creditsCommands;

        private readonly Mock<IUserRepository> _userRepository = new Mock<IUserRepository>();
        private readonly Mock<ISlurRepository> _slurRepository = new Mock<ISlurRepository>();
        private readonly Mock<ISlackWebApi> _slack = new Mock<ISlackWebApi>();

        [TestInitialize]
        public void Setup()
        {
            _userRepository.Setup(repo => repo.GetUsers()).Returns(new List<User>() { new User { Id = "test1", Credits = 23 }, new User { Id = "test2", Credits = 19 }, new User { Id = "test3", Credits = 69 }, new User { Id = "test4", Credits = 43 }, new User { Id = "DESS", Credits = 0 } });

            _creditsCommands = new CreditsCommands(_userRepository.Object, _slack.Object, _slurRepository.Object);
        }

        [TestMethod]
        public void ShouldSendAMessage()
        {
            var command = new Command()
            {
                ChannelId = Channel,
                Text = "",
                UserId = User
            };

             _creditsCommands.Leaderboard(command);

            _slack.Verify(slack => slack.SendMessage(It.IsAny<string>(), command.ChannelId));
        }
    }
}
