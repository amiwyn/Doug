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
    public class JoinCoffeeCommandTest
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

        [TestInitialize]
        public void Setup()
        {
            _coffeeCommands = new CoffeeCommands(_coffeeRepository.Object, _userRepository.Object, _slack.Object, _adminValidator.Object, _coffeeBreakService.Object);
        }

        [TestMethod]
        public void WhenJoiningCoffee_UserIsAddedToRoster()
        {
            _coffeeCommands.JoinCoffee(_command);

            _coffeeRepository.Verify(repo => repo.AddToRoster(User));
        }

        [TestMethod]
        public void WhenJoiningCoffee_UserIsAddedToDatabase()
        {
            _coffeeCommands.JoinCoffee(_command);

            _userRepository.Verify(userRepo => userRepo.AddUser(User));
        }

        [TestMethod]
        public void WhenJoiningCoffee_BroadcastIsSentToChannel()
        {
            _coffeeCommands.JoinCoffee(_command);

            _slack.Verify(slack => slack.SendMessage(It.IsAny<string>(), Channel));
        }
    }
}
