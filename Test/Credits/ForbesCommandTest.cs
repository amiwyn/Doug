using System.Collections.Generic;
using Doug.Commands;
using Doug.Models;
using Doug.Repositories;
using Doug.Services;
using Doug.Services.MenuServices;
using Doug.Slack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Test.Credits
{
    [TestClass]
    public class ForbesCommandTest
    {
        private const string Channel = "coco-channel";
        private const string User = "testuser";

        private CreditsCommands _creditsCommands;

        private readonly Mock<IUserRepository> _userRepository = new Mock<IUserRepository>();
        private readonly Mock<ISlackWebApi> _slack = new Mock<ISlackWebApi>();
        private readonly Mock<IUserService> _userService = new Mock<IUserService>();
        private readonly Mock<IShopMenuService> _shopMenuService = new Mock<IShopMenuService>();

        [TestInitialize]
        public void Setup()
        {
            _userRepository.Setup(repo => repo.GetUsers()).Returns(new List<User>());

            _creditsCommands = new CreditsCommands(_userRepository.Object, _slack.Object, _userService.Object, _shopMenuService.Object);
        }

        [TestMethod]
        public void WhenCheckingForbes_GetInformationFromUsers()
        {
            var command = new Command()
            {
                ChannelId = Channel,
                Text = string.Empty,
                UserId = User
            };

            _creditsCommands.Forbes(command);

            _userRepository.Verify(repo => repo.GetUsers());
        }
    }
}
