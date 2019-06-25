using Doug.Items.Consumables;
using Doug.Models;
using Doug.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Test.Items.Consumables
{
    [TestClass]
    public class CoffeeCupTest
    {
        private CoffeeCup _coffeeCup;

        private readonly Mock<IInventoryRepository> _inventoryRepository = new Mock<IInventoryRepository>();
        private readonly Mock<IStatsRepository> _statsRepository = new Mock<IStatsRepository>();

        private readonly User _user = new User() { Id = "ginette", Energy = 0 };

        [TestInitialize]
        public void Setup()
        {
            _coffeeCup = new CoffeeCup(_statsRepository.Object, _inventoryRepository.Object);
        }

        [TestMethod]
        public void WhenConsuming_IncreaseEnergyBy25()
        {
            _coffeeCup.Use(0, _user);

            _statsRepository.Verify(repo => repo.UpdateEnergy("ginette", 25));
        }

        [TestMethod]
        public void WhenConsuming_ItemIsRemoved()
        {
            _coffeeCup.Use(0, _user);

            _inventoryRepository.Verify(repo => repo.RemoveItem(_user, 0));
        }

        [TestMethod]
        public void GivenHalfFullEnergy_WhenConsuming_ItDoesNotIncreaseOverTheUsersLimitEnergy()
        {
            var user = new User() { Id = "ginette", Energy = 20 };

            _coffeeCup.Use(0, user);

            _statsRepository.Verify(repo => repo.UpdateEnergy("ginette", 25));
        }
    }
}
