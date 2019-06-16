using Doug.Commands;
using Doug.Models;
using Doug.Repositories;
using Doug.Slack;
using Hangfire;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace Test
{
    [TestClass]
    public class StatsCommandTest
    {
        private const string Channel = "coco-channel";
        private const string User = "testuser";

        private CreditsCommands _creditsCommands;

        private readonly Mock<IUserRepository> _userRepository = new Mock<IUserRepository>();
        private readonly Mock<ISlurRepository> _slurRepository = new Mock<ISlurRepository>();
        private readonly Mock<ISlackWebApi> _slack = new Mock<ISlackWebApi>();

        [TestInitialize]
        public void Setup()
        {
            _slurRepository.Setup(repo => repo.GetSlursFrom(It.IsAny<string>())).Returns(new List<Slur>());
            _userRepository.Setup(repo => repo.GetUser(It.IsAny<string>())).Returns(new User());

            _creditsCommands = new CreditsCommands(_userRepository.Object, _slack.Object, _slurRepository.Object);
        }

        [TestMethod]
        public void GivenNoArgument_WhenCheckingStats_GetInformationFromRequester()
        {
            var command = new Command()
            {
                ChannelId = Channel,
                Text = string.Empty,
                UserId = User
            };

            _creditsCommands.Stats(command);

            _userRepository.Verify(repo => repo.GetUser(User));
        }

        [TestMethod]
        public void GivenOneArgument_WhenCheckingStats_GetInformationFromTargetUser()
        {
            var command = new Command()
            {
                ChannelId = Channel,
                Text = "<@otherUserid|username>",
                UserId = User
            };

            _creditsCommands.Stats(command);

            _userRepository.Verify(repo => repo.GetUser("otherUserid"));
        }
    }
}
