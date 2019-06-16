using System.Collections.Generic;
using Doug.Commands;
using Doug.Items;
using Doug.Models;
using Doug.Repositories;
using Doug.Services;
using Doug.Slack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Test.Slurs
{
    [TestClass]
    public class SlursCommandTest
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

        private SlursCommands _slursCommands;

        private readonly Mock<ISlurRepository> _slurRepository = new Mock<ISlurRepository>();
        private readonly Mock<IUserRepository> _userRepository = new Mock<IUserRepository>();
        private readonly Mock<ISlackWebApi> _slack = new Mock<ISlackWebApi>();
        private readonly Mock<IAuthorizationService> _adminValidator = new Mock<IAuthorizationService>();
        private readonly Mock<IItemEventDispatcher> _eventDispatcher = new Mock<IItemEventDispatcher>();

        [TestInitialize]
        public void Setup()
        {
            _slurRepository.Setup(repo => repo.GetSlursFrom(User)).Returns(new List<Slur>() { new Slur("slur", "asdf") });

            _slursCommands = new SlursCommands(_slurRepository.Object, _userRepository.Object, _slack.Object, _adminValidator.Object, _eventDispatcher.Object);
        }

        [TestMethod]
        public void WhenViewingSlurs_SlursAreSpecificToUser()
        {
            _slursCommands.Slurs(_command);

            _slurRepository.Verify(slur => slur.GetSlursFrom(User));
        }
    }
}
