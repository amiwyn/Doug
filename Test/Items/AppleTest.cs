using Doug.Items.Consumables;
using Doug.Models;
using Doug.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;

namespace Test.Items
{
    [TestClass]
    public class AppleTest
    {
        private readonly Mock<IUserRepository> _userRepository = new Mock<IUserRepository>();
        private Apple apple = new Apple();

        private readonly User user = new User() { Id = "test", Health = 90 };

        [TestInitialize]
        public void Setup()
        {
            _userRepository.Setup(repo => repo.GetUsers()).Returns(new List<User>());
        }

        [TestMethod]
        public void WhenUsingRestore25Hitpoint()
        {
            apple.Use(0, user, _userRepository.Object);

            _userRepository.Verify(repo => repo.UpdateHealth("test", 100));
        }

        [TestMethod]
        public void HealthShouldNotBeAbove100()
        {
            apple.Use(0, user, _userRepository.Object);

            Assert.AreNotEqual(115, user.Health);
        }

        [TestMethod]
        public void ShouldRemoveFromInventory()
        {
            apple.Use(0, user, _userRepository.Object);

            _userRepository.Verify(repo => repo.RemoveItem("test", 0));
        }
    }
}
