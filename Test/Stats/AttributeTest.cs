using Doug.Models;
using Doug.Repositories;
using Doug.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Test.Stats
{
    [TestClass]
    public class AttributeTest
    {
        private const string Channel = "coco-channel";
        private const string User = "testuser";

        private readonly Interaction _interaction = new Interaction
        {
            ChannelId = Channel,
            UserId = User,
            Action = "attribution",
            Value = "luck"
        };

        private StatsService _userService;

        private readonly Mock<IUserRepository> _userRepository = new Mock<IUserRepository>();
        private readonly Mock<IStatsRepository> _statsRepository = new Mock<IStatsRepository>();
        private User _user;

        [TestInitialize]
        public void Setup()
        {
            _user = new User() { Id = "testuser", Experience = 0 };
            _userRepository.Setup(repo => repo.GetUser(User)).Returns(_user);

            _userService = new StatsService(_statsRepository.Object, _userRepository.Object);
        }

        [TestMethod]
        public void WhenAttributingLuck_LuckStatIncrease()
        {
            _userService.AttributeStatPoint(_interaction);

            _statsRepository.Verify(repo => repo.AttributeStatPoint(User, "luck"));
        }

        [TestMethod]
        public void GivenUserHasNoFreePoints_WhenAttributingLuck_LuckStatDoNotIncrease()
        {
            _userRepository.Setup(repo => repo.GetUser(User)).Returns(new User() { Id = "testuser", Luck = 10 });

            _userService.AttributeStatPoint(_interaction);

            _statsRepository.Verify(repo => repo.AttributeStatPoint(User, "luck"), Times.Never);
        }
    }
}
