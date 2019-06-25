using Doug.Repositories;
using Doug.Services;
using Doug.Slack;
using Hangfire;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using Doug.Items;
using Doug.Models;

namespace Test
{
    [TestClass]
    public class CoffeeServiceTest
    {
        private const string Channel = "coco-channel";
        private const string User = "testuser";

        private CoffeeService _coffeeService;

        private readonly Mock<ISlackWebApi> _slack = new Mock<ISlackWebApi>();
        private readonly Mock<ICoffeeRepository> _coffeeRepository = new Mock<ICoffeeRepository>();
        private readonly Mock<IChannelRepository> _channelRepository = new Mock<IChannelRepository>();
        private readonly Mock<IBackgroundJobClient> _backgroundJobClient = new Mock<IBackgroundJobClient>();
        private readonly Mock<IUserRepository> _userRepository = new Mock<IUserRepository>();
        private readonly Mock<IInventoryRepository> _inventoryRepository = new Mock<IInventoryRepository>();
        private readonly Mock<IUserService> _userService = new Mock<IUserService>();

        [TestInitialize]
        public void Setup()
        {
            _userRepository.Setup(repo => repo.GetUser(It.IsAny<string>())).Returns(new User());
            _coffeeService = new CoffeeService(_slack.Object, _coffeeRepository.Object, _channelRepository.Object, _backgroundJobClient.Object, _userRepository.Object, _inventoryRepository.Object, _userService.Object);
        }

        [TestMethod]
        public void GivenRightTime_WhenCountingParrotOfUser_UserIsReady()
        {
            _coffeeRepository.Setup(repo => repo.GetMissingParticipants()).Returns(new List<string>() { "bob" });
            var time = new DateTime(1, 1, 1, 18, 0, 0);

            _coffeeService.CountParrot(User, Channel, time);

            _coffeeRepository.Verify(repo => repo.ConfirmUserReady(User));
        }

        [TestMethod]
        public void GivenWrongTime_WhenCountingParrotOfUser_UserIsNotReady()
        {
            _coffeeRepository.Setup(repo => repo.GetMissingParticipants()).Returns(new List<string>() { "bob" });
            var time = new DateTime(1, 1, 1, 19, 0, 0);

            _coffeeService.CountParrot(User, Channel, time);

            _coffeeRepository.Verify(repo => repo.ConfirmUserReady(User), Times.Never);
        }

        [TestMethod]
        public void GivenRightTime_AndEveryoneIsReady_WhenCountingParrotOfUser_BroadcastStart()
        {
            _coffeeRepository.Setup(repo => repo.GetMissingParticipants()).Returns(new List<string>());
            var time = new DateTime(1, 1, 1, 18, 0, 0);

            _coffeeService.CountParrot(User, Channel, time);

            _slack.Verify(slack => slack.BroadcastMessage("Alright, let's do this. <!here> GO!", Channel));
        }

        [TestMethod]
        public void GivenRightTime_ButNotEveryoneIsReady_WhenCountingParrotOfUser_DontBroadcastStart()
        {
            _coffeeRepository.Setup(repo => repo.GetMissingParticipants()).Returns(new List<string>() { "bob" });
            var time = new DateTime(1, 1, 1, 18, 0, 0);

            _coffeeService.CountParrot(User, Channel, time);

            _slack.Verify(slack => slack.BroadcastMessage(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public void GivenOneReady_AndGivenOneNotReady_WhenReminding_SendOneOverTwoRemind()
        {
            _coffeeRepository.Setup(repo => repo.GetMissingParticipants()).Returns(new List<string>() { "bob" });
            _coffeeRepository.Setup(repo => repo.GetReadyParticipants()).Returns(new List<User>() { new User() { Id = "robert" }});

            _coffeeService.CoffeeRemind(Channel);

            _slack.Verify(slack => slack.BroadcastMessage("*1/2* - <@bob> ", Channel));
        }

        [TestMethod]
        public void WhenEndingBreak_RosterIsCleared()
        {
            _coffeeRepository.Setup(repo => repo.GetReadyParticipants()).Returns(new List<User>());

            _coffeeService.EndCoffee(Channel);

            _coffeeRepository.Verify(repo => repo.ResetRoster());
        }

        [TestMethod]
        public void WhenEndingBreak_BroadcastEnd()
        {
            _coffeeRepository.Setup(repo => repo.GetReadyParticipants()).Returns(new List<User>());

            _coffeeService.EndCoffee(Channel);

            _slack.Verify(slack => slack.BroadcastMessage("<!here> Go back to work, ya bunch o' lazy dogs!", Channel));
        }

        [TestMethod]
        public void WhenEndingBreak_ParticipantsGet10Credits()
        {
            _coffeeRepository.Setup(repo => repo.GetReadyParticipants()).Returns(new List<User>());

            _coffeeService.EndCoffee(Channel);

            _userRepository.Verify(repo => repo.AddCreditsToUsers(It.IsAny<List<string>>(), 10));
        }

        [TestMethod]
        public void WhenEndingBreak_ParticipantsGetACoffee()
        {
            _coffeeRepository.Setup(repo => repo.GetReadyParticipants()).Returns(new List<User>());

            _coffeeService.EndCoffee(Channel);

            _inventoryRepository.Verify(repo => repo.AddItemToUsers(It.IsAny<List<string>>(), ItemFactory.CoffeeCup));
        }

        [TestMethod]
        public void WhenEndingBreak_ParticipantsGet300Experience()
        {
            _coffeeRepository.Setup(repo => repo.GetReadyParticipants()).Returns(new List<User>());

            _coffeeService.EndCoffee(Channel);

            _userService.Verify(service => service.AddBulkExperience(It.IsAny<List<User>>(), 300, Channel));
        }
    }
}
