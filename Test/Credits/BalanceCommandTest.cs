using Doug;
using Doug.Commands;
using Doug.Models;
using Doug.Repositories;
using Doug.Slack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Test.Credits
{
    [TestClass]
    public class BalanceCommandTest
    {
        private const string CommandText = "<@otherUserid|username>";
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
        private readonly Mock<ISlurRepository> _slurRepository = new Mock<ISlurRepository>();
        private readonly Mock<ISlackWebApi> _slack = new Mock<ISlackWebApi>();

        [TestInitialize]
        public void Setup()
        {
            _userRepository.Setup(repo => repo.GetUser(User)).Returns(new User() { Id = "bobob", Credits = 79});

            _creditsCommands = new CreditsCommands(_userRepository.Object, _slack.Object, _slurRepository.Object);
        }

        [TestMethod]
        public void WhenCheckingBalance_GetInformationFromUser()
        {
            _creditsCommands.Balance(_command);

            _userRepository.Verify(repo => repo.GetUser(User));
        }

        [TestMethod]
        public void WhenCheckingBalance_MessageIsSentPrivately()
        {
            var message = _creditsCommands.Balance(_command);

            Assert.AreEqual("You have 79 " + DougMessages.CreditEmoji, message.Message);
        }
    }
}
