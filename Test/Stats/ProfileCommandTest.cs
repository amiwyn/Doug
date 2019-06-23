using System.Collections.Generic;
using Doug.Commands;
using Doug.Models;
using Doug.Repositories;
using Doug.Slack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Test.Stats
{
    [TestClass]
    public class ProfileCommandTest
    {
        private const string Channel = "coco-channel";
        private const string User = "testuser";

        private StatsCommands _statsCommands;

        private readonly Mock<IUserRepository> _userRepository = new Mock<IUserRepository>();
        private readonly Mock<ISlurRepository> _slurRepository = new Mock<ISlurRepository>();
        private readonly Mock<ISlackWebApi> _slack = new Mock<ISlackWebApi>();

        [TestInitialize]
        public void Setup()
        {
            _slurRepository.Setup(repo => repo.GetSlursFrom(It.IsAny<string>())).Returns(new List<Slur>());
            _userRepository.Setup(repo => repo.GetUser(It.IsAny<string>())).Returns(new User() { Loadout = new Loadout(null, null, null, null, null, null, null, null)});

            _statsCommands = new StatsCommands(_userRepository.Object, _slack.Object, _slurRepository.Object);
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

            _statsCommands.Profile(command);

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

            _statsCommands.Profile(command);

            _userRepository.Verify(repo => repo.GetUser("otherUserid"));
        }
    }
}
