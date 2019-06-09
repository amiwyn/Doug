using Doug.Commands;
using Doug.Models;
using Doug.Repositories;
using Doug.Services;
using Doug.Slack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Test
{
    [TestClass]
    public class CleanSlursCommandTest
    {
        private const string Channel = "coco-channel";
        private const string User = "testuser";

        private readonly Command command = new Command()
        {
            ChannelId = Channel,
            Text = string.Empty,
            UserId = User
        };

        private SlursCommands _slursCommands;

        private readonly Mock<ISlurRepository> _slurRepository = new Mock<ISlurRepository>();
        private readonly Mock<IUserRepository> _userRepository = new Mock<IUserRepository>();
        private readonly Mock<ISlackWebApi> _slack = new Mock<ISlackWebApi>();
        private readonly Mock<IAdminValidator> _adminValidator = new Mock<IAdminValidator>();

        [TestInitialize]
        public void Setup()
        {
            _slurRepository.Setup(repo => repo.GetRecentSlurs()).Returns(new List<RecentFlame>() { new RecentFlame() { TimeStamp = "6969.6969" } });

            _slack.Setup(slack => slack.GetReactions("6969.6969", Channel)).Returns(Task.FromResult(new List<Reaction>()
            {
                new Reaction() { Name = "+1", Count = 3 },
                new Reaction() { Name = "-1", Count = 5 }
            }));

            _slursCommands = new SlursCommands(_slurRepository.Object, _userRepository.Object, _slack.Object, _adminValidator.Object);
        }

        [TestMethod]
        public async Task WhenCleaning_ConfirmUserIsadmin()
        {
            await _slursCommands.Clean(command);

            _adminValidator.Verify(validator => validator.ValidateUserIsAdmin(User));
        }

        [TestMethod]
        public async Task GivenUserIsAdmin_WhenCleaning_OnlyRecentSlursAreConsidered()
        {
            await _slursCommands.Clean(command);

            _slurRepository.Verify(repo => repo.GetRecentSlurs());
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task GivenGoodRatedSlurs_WhenCleaning_SlursAreNotRemoved()
        {
            _slack.Setup(slack => slack.GetReactions("6969.6969", Channel)).Returns(Task.FromResult(new List<Reaction>()
            {
                new Reaction() { Name = "+1", Count = 4 },
                new Reaction() { Name = "-1", Count = 3 }
            }));

            await _slursCommands.Clean(command);

            _slurRepository.Verify(repo => repo.RemoveSlur(It.IsAny<int>()), Times.Never());
        }

        [TestMethod]
        public async Task GivenBadRatedSlurs_WhenCleaning_SlursAreRemoved()
        {
            await _slursCommands.Clean(command);

            _slurRepository.Verify(repo => repo.RemoveSlur(It.IsAny<int>()));
        }


    }
}
