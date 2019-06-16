using Doug.Commands;
using Doug.Items;
using Doug.Models;
using Doug.Repositories;
using Doug.Services;
using Doug.Slack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Test
{
    [TestClass]
    public class AddSlurCommandTest
    {
        private const string CommandText = "heheahahasod";
        private const string Channel = "coco-channel";
        private const string User = "testuser";

        private readonly Command command = new Command()
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

        [TestInitialize]
        public void Setup()
        { 
            _slursCommands = new SlursCommands(_slurRepository.Object, _userRepository.Object, _slack.Object, _adminValidator.Object, _eventDispatcher.Object);
        }

        [TestMethod]
        public void WhenAddingASlur_SlurIsAdded()
        {
            _slursCommands.AddSlur(command);

            _slurRepository.Verify(repo => repo.AddSlur(It.IsAny<Slur>()));
        }

        [TestMethod]
        public void WhenAddingASlur_UserGetTwoRupee()
        {
            _slursCommands.AddSlur(command);

            _userRepository.Verify(repo => repo.AddCredits(User, 2));
        }
    }
}
