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
    public class JoinSomeoneCommandTest
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

        private readonly Mock<ICoffeeRepository> _coffeeRepository = new Mock<ICoffeeRepository>();
        private readonly Mock<IUserRepository> _userRepository = new Mock<IUserRepository>();
        private readonly Mock<ISlackWebApi> _slack = new Mock<ISlackWebApi>();
        private readonly Mock<IAuthorizationService> _adminValidator = new Mock<IAuthorizationService>();
        private readonly Mock<ICoffeeService> _coffeeBreakService = new Mock<ICoffeeService>();

        [TestInitialize]
        public void Setup()
        {
            _adminValidator.Setup(admin => admin.IsUserSlackAdmin(User)).Returns(Task.FromResult(true));
            _coffeeCommands = new CoffeeCommands(_coffeeRepository.Object, _userRepository.Object, _slack.Object, _adminValidator.Object, _coffeeBreakService.Object);
        }

        [TestMethod]
        public async Task GivenUserIsAdmin_WhenJoiningOtherInCoffee_UserIsAddedToRoster()
        {
            await _coffeeCommands.JoinSomeone(command);

            _coffeeRepository.Verify(repo => repo.AddToRoster("otherUserid"));
        }

        [TestMethod]
        public async Task GivenUserIsAdmin_WhenJoiningOtherInCoffee_UserIsAddedToDatabase()
        {
            await _coffeeCommands.JoinSomeone(command);

            _userRepository.Verify(userRepo => userRepo.AddUser("otherUserid"));
        }

        [TestMethod]
        public async Task GivenUserIsAdmin_WhenJoiningOtherInCoffee_BroadcastIsSentToChannel()
        {
            await _coffeeCommands.JoinSomeone(command);

            _slack.Verify(slack => slack.SendMessage(It.IsAny<string>(), Channel));
        }

        [TestMethod]
        public async Task GivenUserIsNotAdmin_WhenJoiningOtherInCoffee_DontAddUserToRoster()
        {
            _adminValidator.Setup(admin => admin.IsUserSlackAdmin(User)).Returns(Task.FromResult(false));

            await _coffeeCommands.JoinSomeone(command);

            _coffeeRepository.Verify(repo => repo.AddToRoster(It.IsAny<string>()), Times.Never());
        }

        [TestMethod]
        public async Task GivenUserIsNotAdmin_WhenJoiningOtherInCoffee_DontAddUser()
        {
            _adminValidator.Setup(admin => admin.IsUserSlackAdmin(User)).Returns(Task.FromResult(false));

            await _coffeeCommands.JoinSomeone(command);

            _userRepository.Verify(userRepo => userRepo.AddUser(It.IsAny<string>()), Times.Never());
        }

        [TestMethod]
        public async Task GivenUserIsNotAdmin_WhenJoiningOtherInCoffee_DontBroadcastMessage()
        {
            _adminValidator.Setup(admin => admin.IsUserSlackAdmin(User)).Returns(Task.FromResult(false));

            await _coffeeCommands.JoinSomeone(command);

            _slack.Verify(slack => slack.SendMessage(It.IsAny<string>(), Channel), Times.Never());
        }
    }
}
