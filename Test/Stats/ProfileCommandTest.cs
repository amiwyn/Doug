using System.Collections.Generic;
using System.Threading.Tasks;
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
            _userRepository.Setup(repo => repo.GetUser(It.IsAny<string>())).Returns(new User() { Loadout = new Loadout()});

            _statsCommands = new StatsCommands(_userRepository.Object, _slack.Object);
        }

        [TestMethod]
        public async Task GivenNoArgument_WhenCheckingStats_GetInformationFromRequester()
        {
            var command = new Command()
            {
                ChannelId = Channel,
                Text = string.Empty,
                UserId = User
            };

            await _statsCommands.Profile(command);

            _userRepository.Verify(repo => repo.GetUser(User));
        }
    }
}
