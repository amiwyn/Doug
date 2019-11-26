using System.Collections.Generic;
using System.Threading.Tasks;
using Doug;
using Doug.Commands;
using Doug.Effects;
using Doug.Models;
using Doug.Models.Slurs;
using Doug.Repositories;
using Doug.Services;
using Doug.Slack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Test.Slurs
{
    [TestClass]
    public class CleanSlursCommandTest
    {
        private const string Channel = "coco-channel";
        private const string User = "testuser";

        private readonly Command _command = new Command()
        {
            ChannelId = Channel,
            Text = string.Empty,
            UserId = User
        };

        private SlursCommands _slursCommands;

        private readonly Mock<ISlurRepository> _slurRepository = new Mock<ISlurRepository>();
        private readonly Mock<IUserRepository> _userRepository = new Mock<IUserRepository>();
        private readonly Mock<ISlackWebApi> _slack = new Mock<ISlackWebApi>();
        private readonly Mock<IAuthorizationService> _adminValidator = new Mock<IAuthorizationService>();
        private readonly Mock<IEventDispatcher> _eventDispatcher = new Mock<IEventDispatcher>();
        private readonly Mock<IUserService> _userService = new Mock<IUserService>();
        private readonly Mock<ICreditsRepository> _creditsRepository = new Mock<ICreditsRepository>();

        [TestInitialize]
        public void Setup()
        {
            _adminValidator.Setup(admin => admin.IsUserSlackAdmin(User)).Returns(Task.FromResult(true));

            _slurRepository.Setup(repo => repo.GetRecentSlurs()).Returns(new List<RecentFlame>() { new RecentFlame() { TimeStamp = "6969.6969" } });

            _slurRepository.Setup(repo => repo.GetSlur(It.IsAny<int>())).Returns(new Slur("ffff", "robee"));

            _slack.Setup(slack => slack.GetReactions("6969.6969", Channel)).Returns(Task.FromResult(new List<Reaction>()
            {
                new Reaction() { Name = "+1", Count = 3 },
                new Reaction() { Name = "-1", Count = 5 }
            }));

            _slursCommands = new SlursCommands(_slurRepository.Object, _userRepository.Object, _slack.Object, _adminValidator.Object, _eventDispatcher.Object, _userService.Object, _creditsRepository.Object);
        }

        [TestMethod]
        public async Task WhenCleaning_ConfirmUserIsadmin()
        {
            await _slursCommands.Clean(_command);

            _adminValidator.Verify(validator => validator.IsUserSlackAdmin(User));
        }

        [TestMethod]
        public async Task GivenUserIsAdmin_WhenCleaning_OnlyRecentSlursAreConsidered()
        {
            await _slursCommands.Clean(_command);

            _slurRepository.Verify(repo => repo.GetRecentSlurs());
        }

        [TestMethod]
        public async Task GivenUserIsNotAdmin_WhenCleaning_ThenItReturnErrorMessage()
        {
            _adminValidator.Setup(admin => admin.IsUserSlackAdmin(User)).Returns(Task.FromResult(false));

            var response = await _slursCommands.Clean(_command);

            Assert.AreEqual(DougMessages.NotAnAdmin, response.Message);
        }

        

        [TestMethod]
        public async Task GivenGoodRatedSlurs_WhenCleaning_SlursAreNotRemoved()
        {
            _slack.Setup(slack => slack.GetReactions("6969.6969", Channel)).Returns(Task.FromResult(new List<Reaction>()
            {
                new Reaction() { Name = "+1", Count = 4 },
                new Reaction() { Name = "-1", Count = 3 }
            }));

            await _slursCommands.Clean(_command);

            _slurRepository.Verify(repo => repo.RemoveSlur(It.IsAny<int>()), Times.Never());
        }

        [TestMethod]
        public async Task GivenBadRatedSlurs_WhenCleaning_SlursAreRemoved()
        {
            await _slursCommands.Clean(_command);

            _slurRepository.Verify(repo => repo.RemoveSlur(It.IsAny<int>()));
        }

        [TestMethod]
        public async Task WhenCleaning_RecentSlursAreCleared()
        {
            await _slursCommands.Clean(_command);

            _slurRepository.Verify(repo => repo.ClearRecentSlurs());
        }

        [TestMethod]
        public async Task GivenBadRatedSlurs_WhenCleaning_AttachmentIsSent()
        {
            await _slursCommands.Clean(_command);

            _slack.Verify(slack => slack.SendAttachments(It.IsAny<List<Attachment>>(), Channel));
        }
    }
}
