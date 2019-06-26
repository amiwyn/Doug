using Doug.Items.Consumables;
using Doug.Models;
using Doug.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Test.Items.Consumables
{
    [TestClass]
    public class AppleTest
    {
        private const string Channel = "coco-channel";

        private readonly Mock<IInventoryRepository> _inventoryRepository = new Mock<IInventoryRepository>();
        private readonly Mock<IStatsRepository> _statsRepository = new Mock<IStatsRepository>();
        private Apple _apple;

        private readonly User _user = new User() { Id = "test", Health = 90,  };

        [TestInitialize]
        public void Setup()
        {
            _apple = new Apple(_statsRepository.Object, _inventoryRepository.Object);
        }

        [TestMethod]
        public void WhenUsingRestore25Hitpoint()
        {
            _apple.Use(0, _user, Channel);

            _statsRepository.Verify(repo => repo.UpdateHealth("test", 100));
        }

        [TestMethod]
        public void HealthShouldNotBeAbove100()
        {
            _apple.Use(0, _user, Channel);

            Assert.AreNotEqual(115, _user.Health);
        }

        [TestMethod]
        public void ShouldRemoveFromInventory()
        {
            _apple.Use(0, _user, Channel);

            _inventoryRepository.Verify(repo => repo.RemoveItem(_user, 0));
        }
    }
}
