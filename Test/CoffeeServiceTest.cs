using Doug.Repositories;
using Doug.Services;
using Doug.Slack;
using Hangfire;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

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
        private readonly Mock<IBackgroundJobClient> _backgrounJobClient = new Mock<IBackgroundJobClient>();

        [TestInitialize]
        public void Setup()
        {
            _coffeeService = new CoffeeService(_slack.Object, _coffeeRepository.Object, _channelRepository.Object, _backgrounJobClient.Object);
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

            _slack.Verify(slack => slack.SendMessage("Alright, let's do this. <!here> GO!", Channel));
        }

        [TestMethod]
        public void GivenRightTime_ButNotEveryoneIsReady_WhenCountingParrotOfUser_DontBroadcastStart()
        {
            _coffeeRepository.Setup(repo => repo.GetMissingParticipants()).Returns(new List<string>() { "bob" });
            var time = new DateTime(1, 1, 1, 18, 0, 0);

            _coffeeService.CountParrot(User, Channel, time);

            _slack.Verify(slack => slack.SendMessage(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public void GivenOneReady_AndGivenOneNotready_WhenReminding_SendOneOverTwoRemind()
        {
            _coffeeRepository.Setup(repo => repo.GetMissingParticipants()).Returns(new List<string>() { "bob" });
            _coffeeRepository.Setup(repo => repo.GetReadyParticipants()).Returns(new List<string>() { "robert" });

            _coffeeService.CoffeeRemind(Channel);

            _slack.Verify(slack => slack.SendMessage("*1/2* - <@bob> ", Channel));
        }

        [TestMethod]
        public void WhenEndingBreak_RosterIsCleared()
        {
            _coffeeService.EndCoffee(Channel);

            _coffeeRepository.Verify(repo => repo.ResetRoster());
        }

        [TestMethod]
        public void WhenEndingBreak_BroadcastEnd()
        {
            _coffeeService.EndCoffee(Channel);

            _slack.Verify(slack => slack.SendMessage("<!here> Go back to work, ya bunch o' lazy dogs!", Channel));
        }

    }
}
