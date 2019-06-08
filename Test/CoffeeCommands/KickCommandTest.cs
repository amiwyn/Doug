using Doug;
using Doug.Commands;
using Doug.Models;
using Doug.Repositories;
using Doug.Services;
using Doug.Slack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;

namespace Test
{
    [TestClass]
    public class KickCommandTest
    {
        private const string CommandText = "<@otherUserid|username>";
        private const string Channel = "testchannel";
        private const string User = "testuser";

        private readonly Command command = new Command()
        {
            ChannelId = Channel,
            Text = CommandText,
            UserId = User
        };

        private CoffeeCommands _coffeeCommands;

        private readonly Mock<IChannelRepository> _channelRepository = new Mock<IChannelRepository>();
        private readonly Mock<IUserRepository> _userRepository = new Mock<IUserRepository>();
        private readonly Mock<ISlackWebApi> _slack = new Mock<ISlackWebApi>();
        private readonly Mock<IAdminValidator> _adminValidator = new Mock<IAdminValidator>();
        private readonly Mock<ICoffeeBreakService> _coffeeBreakService = new Mock<ICoffeeBreakService>();

        [TestInitialize]
        public void Setup()
        {
            _coffeeCommands = new CoffeeCommands(_channelRepository.Object, _userRepository.Object, _slack.Object, _adminValidator.Object, _coffeeBreakService.Object);
        }

        [TestMethod]
        public async Task GivenUserIsAdmin_WhenKickingUser_UserIsRemovedFromRoster()
        {
            await _coffeeCommands.KickCoffee(command);

            _channelRepository.Verify(channelRepo => channelRepo.RemoveFromRoster("otherUserid"));
        }

        [TestMethod]
        public async Task GivenUserIsAdmin_WhenKickingUser_BroadcastIsSentToChannel()
        {
            await _coffeeCommands.KickCoffee(command);

            _slack.Verify(slack => slack.SendMessage(It.IsAny<string>(), Channel));
        }

        [TestMethod]
        [ExpectedException(typeof(UserNotAdminException))]
        public async Task GivenUserIsNotAdmin_WhenKickingUser_DontAddUserToRoster()
        {
            _adminValidator.Setup(validator => validator.ValidateUserIsAdmin(User)).Throws(new UserNotAdminException());

            await _coffeeCommands.KickCoffee(command);

            _channelRepository.Verify(channelRepo => channelRepo.RemoveFromRoster(It.IsAny<string>()), Times.Never());
        }

        [TestMethod]
        [ExpectedException(typeof(UserNotAdminException))]
        public async Task GivenUserIsNotAdmin_WhenKickingUser_DontBroadcastMessage()
        {
            _adminValidator.Setup(validator => validator.ValidateUserIsAdmin(User)).Throws(new UserNotAdminException());

            await _coffeeCommands.KickCoffee(command);

            _slack.Verify(slack => slack.SendMessage(It.IsAny<string>(), Channel), Times.Never());
        }
    }
}
