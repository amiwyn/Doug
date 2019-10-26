using Doug;
using Doug.Commands;
using Doug.Models;
using Doug.Models.User;
using Doug.Repositories;
using Doug.Slack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Test.Stats
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

        private StatsCommands _statsCommands;

        private readonly Mock<IUserRepository> _userRepository = new Mock<IUserRepository>();
        private readonly Mock<ISlackWebApi> _slack = new Mock<ISlackWebApi>();
        private readonly Mock<IPartyRepository> _partyRepository = new Mock<IPartyRepository>();

        [TestInitialize]
        public void Setup()
        {
            _userRepository.Setup(repo => repo.GetUser(User)).Returns(new User() { Id = "bobob", Credits = 79});

            _statsCommands = new StatsCommands(_userRepository.Object, _slack.Object, _partyRepository.Object);
        }

        [TestMethod]
        public void WhenCheckingBalance_GetInformationFromUser()
        {
            _statsCommands.Balance(_command);

            _userRepository.Verify(repo => repo.GetUser(User));
        }

        [TestMethod]
        public void WhenCheckingBalance_MessageIsSentPrivately()
        {
            var message = _statsCommands.Balance(_command);

            Assert.AreEqual("You have 79 " + DougMessages.CreditEmoji, message.Message);
        }
    }
}
