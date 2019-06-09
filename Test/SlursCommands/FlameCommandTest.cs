using Doug.Commands;
using Doug.Models;
using Doug.Repositories;
using Doug.Services;
using Doug.Slack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Test
{
    [TestClass]
    public class FlameCommandTest
    {
        private const string CommandText = "<@otherUserid|username>";
        private const string Channel = "coco-channel";
        private const string User = "testuser";

        private readonly Command command = new Command()
        {
            ChannelId = Channel,
            Text = CommandText,
            UserId = User
        };

        private SlursCommands _slursCommands;

        private readonly Mock<ISlurRepository> _slurRepository = new Mock<ISlurRepository>();
        private readonly Mock<IUserRepository> _userRepository = new Mock<IUserRepository>();
        private readonly Mock<ISlackWebApi> _slack = new Mock<ISlackWebApi>();
        private readonly Mock<IAdminValidator> _adminValidator = new Mock<IAdminValidator>();

        [TestInitialize]
        public void Setup()
        {
            _slurRepository.Setup(repo => repo.GetSlurs()).Returns(new List<Slur>() { new Slur("{user} is a {random} 350++ bitch", "asdf") });
            _userRepository.Setup(repo => repo.GetUsers()).Returns(new List<User>() { new User() { Id = "robert" } });

            _slursCommands = new SlursCommands(_slurRepository.Object, _userRepository.Object, _slack.Object, _adminValidator.Object);
        }

        [TestMethod]
        public async Task GivenASlurWithUserMention_WhenFlaming_UserIsMentionned()
        {
            await _slursCommands.Flame(command);

            _slack.Verify(slack => slack.SendMessage(It.IsRegex("<@otherUserid>"), Channel));
        }

        [TestMethod]
        public async Task GivenASlurWithRandom_WhenFlaming_RandomIsMentionned()
        {
            await _slursCommands.Flame(command);

            _slack.Verify(slack => slack.SendMessage(It.IsRegex("<@robert>"), Channel));
        }

        [TestMethod]
        public async Task GivenASlurWith350_WhenFlaming_DynamicMassIsUsed()
        {
            _slurRepository.Setup(repo => repo.GetFat()).Returns(69);
            await _slursCommands.Flame(command);

            _slack.Verify(slack => slack.SendMessage(It.IsRegex("69"), Channel));
        }

        [TestMethod]
        public async Task GivenASlurWith350_WhenFlaming_350GetsHeavier()
        {
            await _slursCommands.Flame(command);

            _slurRepository.Verify(repo => repo.IncrementFat());
        }

        [TestMethod]
        public async Task GivenSpecificSlur_WhenFlaming_SlurIsSpecific()
        {
            _slurRepository.Setup(repo => repo.GetSlur(It.IsAny<int>())).Returns(new Slur("ha", "ho"));

            var command = new Command()
            {
                ChannelId = Channel,
                Text = CommandText + " 69",
                UserId = User
            };

            await _slursCommands.Flame(command);

            _slurRepository.Verify(repo => repo.GetSlur(69));
        }

        [TestMethod]
        public async Task GivenSpecificSlur_WhenFlaming_5CreditsAreRemoved()
        {
            _slurRepository.Setup(repo => repo.GetSlur(It.IsAny<int>())).Returns(new Slur("he", "he"));

            var command = new Command()
            {
                ChannelId = Channel,
                Text = CommandText + " 69",
                UserId = User
            };

            await _slursCommands.Flame(command);

            _userRepository.Verify(repo => repo.RemoveCredits(User, 5));
        }

        [TestMethod]
        public async Task WhenFlaming_SlurIsLogged()
        {
            _slack.Setup(slack => slack.SendMessage(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult("696969.696969"));

            await _slursCommands.Flame(command);

            _slurRepository.Verify(repo => repo.LogRecentSlur(It.IsAny<int>(), "696969.696969"));
        }
    }
}
