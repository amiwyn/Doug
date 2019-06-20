using Doug;
using Doug.Commands;
using Doug.Items;
using Doug.Models;
using Doug.Repositories;
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
        private readonly Mock<IItemEventDispatcher> _itemEventDispatcher = new Mock<IItemEventDispatcher>();


        [TestInitialize]
        public void Setup()
        {
            _userRepository.Setup(repo => repo.GetUser(User)).Returns(new User { Energy = 10 });
            _userRepository.Setup(repo => repo.GetUser("robert")).Returns(new User { Id = "robert", Credits = 10 });
            _itemEventDispatcher.Setup(disp => disp.OnStealingAmount(It.IsAny<User>(), It.IsAny<int>())).Returns(1);

            _combatCommands = new CombatCommands(_itemEventDispatcher.Object, _userRepository.Object, _slack.Object);
        }

        [TestMethod]
        public void WhenStealing_StealRateIsAroundAQuarter()
        {
            var winCount = 0;
            _userRepository.Setup(repo => repo.AddCredits(User, It.IsAny<int>())).Callback(() => winCount++);
            _itemEventDispatcher.Setup(disp => disp.OnStealingChance(It.IsAny<User>(), It.IsAny<double>())).Returns(0.25);
            _itemEventDispatcher.Setup(disp => disp.OnGettingStolenChance(It.IsAny<User>(), It.IsAny<double>())).Returns(0.75);

            for (int i = 0; i < 5000; i++)
            {
                _combatCommands.Steal(_command);
            }

            Assert.IsTrue(winCount > 1150 && winCount < 1350);
        }

        [TestMethod]
        public void WhenStealingSucceed_AmountIsRemovedFromTheTarget()
        {
            _itemEventDispatcher.Setup(disp => disp.OnStealingChance(It.IsAny<User>(), It.IsAny<double>())).Returns(1);

            _combatCommands.Steal(_command);

            _userRepository.Verify(repo => repo.RemoveCredits("robert", 1));
        }

        [TestMethod]
        public void WhenStealingSucceed_AmountIsAddedToTheStealer()
        {
            _itemEventDispatcher.Setup(disp => disp.OnStealingChance(It.IsAny<User>(), It.IsAny<double>())).Returns(1);

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
        public void GivenTargetHasNotEnoughCredits_WhenStealing_NotEnougCreditsMessage()
        {
            _userRepository.Setup(repo => repo.GetUser("robert")).Returns(new User { Credits = 0 });

            var result = _combatCommands.Steal(_command);

            Assert.AreEqual(DougMessages.TargetNoMoney, result.Message);
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
    }
}
