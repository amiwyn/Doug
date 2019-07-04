using System.Collections.Generic;
using Doug;
using Doug.Commands;
using Doug.Items;
using Doug.Models;
using Doug.Repositories;
using Doug.Services;
using Doug.Slack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Test.Slurs
{
    [TestClass]
    public class WholastCommandTest
    {
        private const string CommandText = "<@otherUserid|username>";
        private const string Channel = "coco-channel";
        private const string User = "testuser";

        private readonly Command _command = new Command()
        {
            ChannelId = Channel,
            Text = CommandText,
            UserId = User
        };

        private SlursCommands _slursCommands;

        private readonly Mock<ISlurRepository> _slurRepository = new Mock<ISlurRepository>();
        private readonly Mock<IUserRepository> _userRepository = new Mock<IUserRepository>();
        private readonly Mock<ISlackWebApi> _slack = new Mock<ISlackWebApi>();
        private readonly Mock<IAuthorizationService> _adminValidator = new Mock<IAuthorizationService>();
        private readonly Mock<IItemEventDispatcher> _eventDispatcher = new Mock<IItemEventDispatcher>();
        private readonly Mock<IUserService> _userService = new Mock<IUserService>();

        [TestInitialize]
        public void Setup()
        {
            _userRepository.Setup(repo => repo.GetUser(User)).Returns(new User() { Id = "testuser", Credits = 68 });

            _slurRepository.Setup(repo => repo.GetRecentSlurs()).Returns(new List<RecentFlame>() { new RecentFlame() });
            _slurRepository.Setup(repo => repo.GetSlur(It.IsAny<int>())).Returns(new Slur("ffff", "asdf"));

            _slursCommands = new SlursCommands(_slurRepository.Object, _userRepository.Object, _slack.Object, _adminValidator.Object, _eventDispatcher.Object, _userService.Object);
        }

        [TestMethod]
        public void WhenCheckingLastSlurAuthor_Remove2CreditsToUser()
        {
            _slursCommands.WhoLast(_command);

            _userRepository.Verify(repo => repo.RemoveCredits(User, 2));
        }

        [TestMethod]
        public void WhenCheckingLastSlurAuthor_LastSlurAuthorIsReturned()
        {
            _userService.Setup(service => service.Mention(It.IsAny<User>())).Returns("<@asdf>");
            var result = _slursCommands.WhoLast(_command);

            Assert.AreEqual("<@asdf> created that slur.", result.Message);
        }

        [TestMethod]
        public void GivenUserHasBotEnoughCredits_WhenCheckingLastSlurAuthor_ErrorMessageIsSent()
        {
            _userRepository.Setup(repo => repo.GetUser(User)).Returns(new User() { Id = "testuser", Credits = 0 });

            var result = _slursCommands.WhoLast(_command);

            Assert.AreEqual("You need 2 " + DougMessages.CreditEmoji + " to do this and you have 0 " + DougMessages.CreditEmoji, result.Message);
        }
    }
}
