using System.Collections.Generic;
using System.Threading.Tasks;
using Doug;
using Doug.Effects;
using Doug.Models;
using Doug.Models.User;
using Doug.Repositories;
using Doug.Services;
using Doug.Skills.Combat;
using Doug.Slack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Test.Combat
{
    [TestClass]
    public class StealTest
    {
        private const string Channel = "coco-channel";

        private readonly User _user = new User { Id = "bebebobo", Energy = 10 , Loadout = new Loadout() };
        private readonly User _target = new User { Id = "robert", Credits = 10 };

        private Steal _steal;

        private readonly Mock<ISlackWebApi> _slack = new Mock<ISlackWebApi>();
        private readonly Mock<IEventDispatcher> _itemEventDispatcher = new Mock<IEventDispatcher>();
        private readonly Mock<IStatsRepository> _statsRepository = new Mock<IStatsRepository>();
        private readonly Mock<IRandomService> _randomService = new Mock<IRandomService>();
        private readonly Mock<IUserService> _userService = new Mock<IUserService>();
        private readonly Mock<IChannelRepository> _channelRepository = new Mock<IChannelRepository>();
        private readonly Mock<ICreditsRepository> _creditsRepository = new Mock<ICreditsRepository>();

        [TestInitialize]
        public void Setup()
        {
            _slack.Setup(slack => slack.GetUsersInChannel("coco-channel")).Returns(Task.FromResult(new List<string> { "robert" }));
            _userService.Setup(service => service.IsUserActive(It.IsAny<string>())).Returns(Task.FromResult(true));
            _channelRepository.Setup(repo => repo.GetChannelType("coco-channel")).Returns(ChannelType.Pvp);
            _itemEventDispatcher.Setup(disp => disp.OnStealingAmount(It.IsAny<User>(), It.IsAny<int>())).Returns(1);

            _steal = new Steal(_statsRepository.Object, _slack.Object, _userService.Object,
                _channelRepository.Object, _itemEventDispatcher.Object, _randomService.Object, _creditsRepository.Object);
        }

        [TestMethod]
        public async Task WhenStealingSucceed_AmountIsRemovedFromTheTarget()
        {
            _randomService.Setup(rnd => rnd.RollAgainstOpponent(It.IsAny<double>(), It.IsAny<double>())).Returns(true);

            await _steal.Activate(_user, _target, Channel);

            _creditsRepository.Verify(repo => repo.RemoveCredits("robert", 1));
        }

        [TestMethod]
        public async Task WhenStealingSucceed_AmountIsAddedToTheStealer()
        {
            _randomService.Setup(rnd => rnd.RollAgainstOpponent(It.IsAny<double>(), It.IsAny<double>())).Returns(true);

            await _steal.Activate(_user, _target, Channel);

            _creditsRepository.Verify(repo => repo.AddCredits(_user.Id, 1));
        }

        [TestMethod]
        public async Task GivenUserHasNoEnergy_WhenStealing_NotEnoughEnergyMessage()
        {
            var user = new User { Energy = 0, Loadout = new Loadout() };

            var result = await _steal.Activate(user, _target, Channel);

            Assert.AreEqual(DougMessages.NotEnoughEnergy, result.Message);
        }

        [TestMethod]
        public async Task GivenTargetHasNotEnoughCredits_WhenStealing_UserLoseAllHisCredits()
        {
            _randomService.Setup(rnd => rnd.RollAgainstOpponent(It.IsAny<double>(), It.IsAny<double>())).Returns(true);
            _itemEventDispatcher.Setup(disp => disp.OnStealingAmount(It.IsAny<User>(), It.IsAny<int>())).Returns(77);
            var target = new User { Id = "robert", Credits = 3 };

            await _steal.Activate(_user, target, Channel);

            _creditsRepository.Verify(repo => repo.RemoveCredits("robert", 3));
        }

        [TestMethod]
        public async Task WhenStealing_ObtainChancesFromItems()
        {
            await _steal.Activate(_user, _target, Channel);

            _itemEventDispatcher.Verify(dispatcher => dispatcher.OnStealingChance(It.IsAny<User>(), It.IsAny<double>()));
        }

        [TestMethod]
        public async Task WhenStealing_ObtainAmountFromItems()
        {
            _itemEventDispatcher.Setup(disp => disp.OnStealingChance(It.IsAny<User>(), It.IsAny<double>())).Returns(1);

            await _steal.Activate(_user, _target, Channel);

            _itemEventDispatcher.Verify(dispatcher => dispatcher.OnStealingAmount(It.IsAny<User>(), It.IsAny<int>()));
        }

        [TestMethod]
        public async Task GivenUserIsInWrongChannel_WhenStealing_WrongChannelMessage()
        {
            _channelRepository.Setup(repo => repo.GetChannelType("coco-channel")).Returns(ChannelType.Casino);

            var result = await _steal.Activate(_user, _target, Channel);

            Assert.AreEqual(DougMessages.NotInRightChannel, result.Message);
        }

    }
}
