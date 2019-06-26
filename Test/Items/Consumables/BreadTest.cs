using Doug.Items.Consumables;
using Doug.Models;
using Doug.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Test.Items.Consumables
{
    [TestClass]
    public class BreadTest
    {
        private const string Channel = "coco-channel";

        private readonly Mock<IInventoryRepository> _inventoryRepository = new Mock<IInventoryRepository>();
        private readonly Mock<IStatsRepository> _statsRepository = new Mock<IStatsRepository>();
        private Bread _bread;

        private readonly User _user = new User() { Id = "ginette", Health = 25 };

        [TestInitialize]
        public void Setup()
        {
            _bread = new Bread(_statsRepository.Object, _inventoryRepository.Object);
        }

        [TestMethod]
        public void WhenConsuming_IncreaseHealthBy50()
        {
            _bread.Use(0, _user, Channel);

            _statsRepository.Verify(repo => repo.UpdateHealth("ginette", 75));
        }

        [TestMethod]
        public void WhenConsuming_ItemIsRemoved()
        {
            _bread.Use(0, _user, Channel);

            _inventoryRepository.Verify(repo => repo.RemoveItem(_user, 0));
        }

        [TestMethod]
        public void WhenConsuming_ItDoesNotIncreaseOverTheUsersLimitHealth()
        {
            var user = new User { Id = "ginette", Health = 77 };

            _bread.Use(0, user, Channel);

            _statsRepository.Verify(repo => repo.UpdateHealth("ginette", 100));
        }
    }
}
