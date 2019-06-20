using System.Collections.Generic;
using Doug.Items.Consumables;
using Doug.Models;
using Doug.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Test.Items
{
    [TestClass]
    public class NormalEnergyDrinkTest
    {
        private NormalEnergyDrink _normalEnergyDrink;

        private readonly Mock<IUserRepository> _userRepository = new Mock<IUserRepository>();

        private readonly User _user = new User() { Id = "ginette", Energy = 0 };

        [TestInitialize]
        public void Setup()
        {
            _userRepository.Setup(repo => repo.GetUsers()).Returns(new List<User>());

            _normalEnergyDrink = new NormalEnergyDrink();
        }

        [TestMethod]
        public void WhenConsuming_IncreaseEnergyBy25()
        {
            _normalEnergyDrink.Use(0, _user, _userRepository.Object);

            _userRepository.Verify(repo => repo.UpdateEnergy("ginette", 25));
        }

        [TestMethod]
        public void WhenConsuming_ItemIsRemoved()
        {
            _normalEnergyDrink.Use(0, _user, _userRepository.Object);

            _userRepository.Verify(repo => repo.RemoveItem("ginette", 0));
        }

        [TestMethod]
        public void GivenHalfFullEnergy_WhenConsuming_ItDoesNotIncreaseOverTheUsersLimitEnergy()
        {
            var user = new User() { Id = "ginette", Energy = 20 };

            _normalEnergyDrink.Use(0, user, _userRepository.Object);

            _userRepository.Verify(repo => repo.UpdateEnergy("ginette", 25));
        }
    }
}
