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
    public class ResolveCommandTest
    {
        private const string CommandText = "<@otherUserid|username>";
        private const string Channel = "testchannel";
        private const string User = "testuser";

        private readonly Command _command = new Command()
        {
            ChannelId = Channel,
            Text = CommandText,
            UserId = User
        };

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
        public async Task GivenUserIsAdmin_WhenResolving_CoffeeIsLaunched()
        {
            _adminValidator.Setup(admin => admin.IsUserSlackAdmin(User)).Returns(Task.FromResult(true));

            await _coffeeCommands.Resolve(_command);

            _coffeeBreakService.Verify(coffeeService => coffeeService.LaunchCoffeeBreak(Channel));
        }

        [TestMethod]
        public async Task GivenUserIsNotAdmin_WhenResolving_CoffeeIsNotLaunched()
        {
            _adminValidator.Setup(admin => admin.IsUserSlackAdmin(User)).Returns(Task.FromResult(false));

            await _coffeeCommands.Resolve(_command);

            _coffeeBreakService.Verify(coffeeService => coffeeService.LaunchCoffeeBreak(Channel), Times.Never());
        }
    }
}
