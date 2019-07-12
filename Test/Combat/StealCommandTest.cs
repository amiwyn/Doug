using Doug;
using Doug.Commands;
using Doug.Items;
using Doug.Models;
using Doug.Repositories;
using Doug.Services;
using Doug.Slack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Test.Combat
{
    [TestClass]
    public class StealCommandTest
    {
        private const string CommandText = "<@robert|asdas>";
        private const string Channel = "coco-channel";
        private const string User = "testuser";

        private readonly Command _command = new Command()
        {
            ChannelId = Channel,
            Text = CommandText,
            UserId = User
        };

        private CombatCommands _combatCommands;

        private readonly Mock<IUserRepository> _userRepository = new Mock<IUserRepository>();
        private readonly Mock<ISlackWebApi> _slack = new Mock<ISlackWebApi>();
        private readonly Mock<IEventDispatcher> _itemEventDispatcher = new Mock<IEventDispatcher>();
        private readonly Mock<IStatsRepository> _statsRepository = new Mock<IStatsRepository>();
        private readonly Mock<IRandomService> _randomService = new Mock<IRandomService>();
        private readonly Mock<IUserService> _userService = new Mock<IUserService>();
        private readonly Mock<IChannelRepository> _channelRepository = new Mock<IChannelRepository>();
        private readonly Mock<IGovernmentService> _governmentService = new Mock<IGovernmentService>();



        [TestInitialize]
        public void Setup()
        {
            _channelRepository.Setup(repo => repo.GetChannelType("coco-channel")).Returns(ChannelType.Common);
            _userRepository.Setup(repo => repo.GetUser(User)).Returns(new User { Energy = 10 });
            _userRepository.Setup(repo => repo.GetUser("robert")).Returns(new User { Id = "robert", Credits = 10 });
            _itemEventDispatcher.Setup(disp => disp.OnStealingAmount(It.IsAny<User>(), It.IsAny<int>())).Returns(1);

            _combatCommands = new CombatCommands(_itemEventDispatcher.Object, _userRepository.Object, _slack.Object, _statsRepository.Object, _randomService.Object, _userService.Object, _channelRepository.Object, _governmentService.Object);
        }

        [TestMethod]
        public void WhenStealingSucceed_AmountIsRemovedFromTheTarget()
        {
            _randomService.Setup(rnd => rnd.RollAgainstOpponent(It.IsAny<double>(), It.IsAny<double>())).Returns(true);

            _combatCommands.Steal(_command);

            _userRepository.Verify(repo => repo.RemoveCredits("robert", 1));
        }

        [TestMethod]
        public void WhenStealingSucceed_AmountIsAddedToTheStealer()
        {
            _randomService.Setup(rnd => rnd.RollAgainstOpponent(It.IsAny<double>(), It.IsAny<double>())).Returns(true);

            _combatCommands.Steal(_command);

            _userRepository.Verify(repo => repo.AddCredits(User, 1));
        }

        [TestMethod]
        public void GivenUserHasNoEnergy_WhenStealing_NotEnoughEnergyMessage()
        {
            _userRepository.Setup(repo => repo.GetUser(User)).Returns(new User { Energy = 0 });

            var result = _combatCommands.Steal(_command);

            Assert.AreEqual(DougMessages.NotEnoughEnergy, result.Message);
        }

        [TestMethod]
        public void GivenTargetHasNotEnoughCredits_WhenStealing_UserLoseAllHisCredits()
        {
            _randomService.Setup(rnd => rnd.RollAgainstOpponent(It.IsAny<double>(), It.IsAny<double>())).Returns(true);
            _itemEventDispatcher.Setup(disp => disp.OnStealingAmount(It.IsAny<User>(), It.IsAny<int>())).Returns(77);
            _userRepository.Setup(repo => repo.GetUser("robert")).Returns(new User { Id = "robert", Credits = 3 });

            _combatCommands.Steal(_command);

            _userRepository.Verify(repo => repo.RemoveCredits("robert", 3));
        }

        [TestMethod]
        public void WhenStealing_ObtainChancesFromItems()
        {
            _combatCommands.Steal(_command);

            _itemEventDispatcher.Verify(dispatcher => dispatcher.OnStealingChance(It.IsAny<User>(), It.IsAny<double>()));
        }

        [TestMethod]
        public void WhenStealing_ObtainAmountFromItems()
        {
            _itemEventDispatcher.Setup(disp => disp.OnStealingChance(It.IsAny<User>(), It.IsAny<double>())).Returns(1);

            _combatCommands.Steal(_command);

            _itemEventDispatcher.Verify(dispatcher => dispatcher.OnStealingAmount(It.IsAny<User>(), It.IsAny<int>()));
        }

        [TestMethod]
        public void GivenUserIsInWrongChannel_WhenStealing_WrongChannelMessage()
        {
            _channelRepository.Setup(repo => repo.GetChannelType("coco-channel")).Returns(ChannelType.Casino);

            var result = _combatCommands.Steal(_command);

            Assert.AreEqual(DougMessages.NotInRightChannel, result.Message);
        }

    }
}
