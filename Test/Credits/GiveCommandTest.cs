using Doug.Commands;
using Doug.Models;
using Doug.Models.User;
using Doug.Repositories;
using Doug.Services;
using Doug.Services.MenuServices;
using Doug.Slack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Test.Credits
{
    [TestClass]
    public class GiveCommandTest
    {
        private const string CommandText = "<@otherUserid|username> 10";
        private const string Channel = "coco-channel";
        private const string User = "testuser";

        private readonly Command _command = new Command()
        {
            ChannelId = Channel,
            Text = CommandText,
            UserId = User
        };

        private CreditsCommands _creditsCommands;

        private readonly Mock<IUserRepository> _userRepository = new Mock<IUserRepository>();
        private readonly Mock<ISlackWebApi> _slack = new Mock<ISlackWebApi>();
        private readonly Mock<IUserService> _userService = new Mock<IUserService>();
        private readonly Mock<IShopMenuService> _shopMenuService = new Mock<IShopMenuService>();
        private readonly Mock<ICreditsRepository> _creditsRepository = new Mock<ICreditsRepository>();

        [TestInitialize]
        public void Setup()
        {
            _userRepository.Setup(repo => repo.GetUser(User)).Returns(new User() { Id = "testuser", Credits = 79});
            _userRepository.Setup(repo => repo.GetUser("otherUserid")).Returns(new User() { Id = "otherUserid", Credits = 79});

            _creditsCommands = new CreditsCommands(_userRepository.Object, _slack.Object, _userService.Object, _shopMenuService.Object, _creditsRepository.Object);
        }

        [TestMethod]
        public void GivenEnoughCredits_WhenGivingCredits_CreditsAreRemovedFromGiver()
        {
            _creditsCommands.Give(_command);

            _creditsRepository.Verify(repo => repo.RemoveCredits(User, 10));
        }

        [TestMethod]
        public void GivenEnoughCredits_WhenGivingCredits_CreditsAreAddedToReceiver()
        {
            _creditsCommands.Give(_command);

            _creditsRepository.Verify(repo => repo.AddCredits("otherUserid", 10));
        }

        [TestMethod]
        public void GivenNotEnoughCredits_WhenGivingCredits_NotEnoughCreditsMessageIsSent()
        {
            _creditsCommands.Give(_command);

            _creditsRepository.Verify(repo => repo.AddCredits(User, It.IsAny<int>()), Times.Never);
        }

        [TestMethod]
        public void GivenEnoughCredits_WhenGivingCredits_MessageIsBroadcasted()
        {
            _creditsCommands.Give(_command);

            _slack.Verify(slack => slack.BroadcastMessage(It.IsAny<string>(), Channel));
        }

        [TestMethod]
        public void GivenNegativeAmount_WhenGivingCredits_()
        {
            var command = new Command()
            {
                ChannelId = Channel,
                Text = "<@otherUserid|username> -10",
                UserId = User
            };

            var result = _creditsCommands.Give(command);

            Assert.AreEqual("Invalid amount", result.Message);
        }
    }
}
