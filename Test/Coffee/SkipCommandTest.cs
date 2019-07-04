using System.Threading.Tasks;
using Doug.Commands;
using Doug.Models;
using Doug.Repositories;
using Doug.Services;
using Doug.Slack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Test.Coffee
{
    [TestClass]
    public class SkipCommandTest
    {
        private const string Channel = "cocoChannel";
        private const string User = "testuser";

        private CoffeeCommands _coffeeCommands;

        private readonly Mock<ICoffeeRepository> _coffeeRepository = new Mock<ICoffeeRepository>();
        private readonly Mock<IUserRepository> _userRepository = new Mock<IUserRepository>();
        private readonly Mock<ISlackWebApi> _slack = new Mock<ISlackWebApi>();
        private readonly Mock<IAuthorizationService> _adminValidator = new Mock<IAuthorizationService>();
        private readonly Mock<ICoffeeService> _coffeeBreakService = new Mock<ICoffeeService>();
        private readonly Mock<IUserService> _userService = new Mock<IUserService>();

        [TestInitialize]
        public void Setup()
        {
            _coffeeCommands = new CoffeeCommands(_coffeeRepository.Object, _userRepository.Object, _slack.Object, _adminValidator.Object, _coffeeBreakService.Object, _userService.Object);
        }

        [TestMethod]
        public async Task GivenCommandIsCalledWithoutArgument_WhenSkipping_UserIsSkipped()
        {
            var command = new Command()
            {
                ChannelId = Channel,
                Text = null,
                UserId = User
            };

            await _coffeeCommands.Skip(command);

            _coffeeRepository.Verify(repo => repo.SkipUser(User));
        }

        [TestMethod]
        public async Task GivenCommandIsCalledWithArgument_AndGivenUserIsAdmin_WhenSkippingTargetUser_TargetIsSkipped()
        {
            _adminValidator.Setup(admin => admin.IsUserSlackAdmin(User)).Returns(Task.FromResult(true));

            var command = new Command()
            {
                ChannelId = Channel,
                Text = "<@otherUserid|username>",
                UserId = User
            };

            await _coffeeCommands.Skip(command);

            _coffeeRepository.Verify(repo => repo.SkipUser("otherUserid"));
        }

        [TestMethod]
        public async Task GivenCommandIsCalledWithArgument_AndGivenUserIsNotAdmin_WhenSkippingTargetUser_TargetIsNotSkipped()
        {
            var command = new Command()
            {
                ChannelId = Channel,
                Text = "<@otherUserid|username>",
                UserId = User
            };

            _adminValidator.Setup(admin => admin.IsUserSlackAdmin(User)).Returns(Task.FromResult(false));

            await _coffeeCommands.Skip(command);

            _coffeeRepository.Verify(repo => repo.SkipUser(It.IsAny<string>()), Times.Never());
        }
    }
}
