﻿using Doug.Items.Consumables;
using Doug.Models;
using Doug.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Test.Items.Consumables
{
    [TestClass]
    public class McdoFriesTest
    {
        private readonly Mock<IInventoryRepository> _inventoryRepository = new Mock<IInventoryRepository>();
        private readonly Mock<IStatsRepository> _statsRepository = new Mock<IStatsRepository>();
        private McdoFries _fries;

        private readonly User _user = new User() { Id = "ginette", Health = 60, Energy = 4, Experience = 4892939 };

        [TestInitialize]
        public void Setup()
        {
            _fries = new McdoFries(_statsRepository.Object, _inventoryRepository.Object);
        }

        [TestMethod]
        public void WhenConsuming_IncreaseEnergyBy50()
        {
            _fries.Use(0, _user);

            _statsRepository.Verify(repo => repo.UpdateEnergy("ginette", 54));
        }

        [TestMethod]
        public void WhenConsuming_DecreaseHealthBy25()
        {
            _fries.Use(0, _user);

            _statsRepository.Verify(repo => repo.UpdateHealth("ginette", 35));
        }

        [TestMethod]
        public void WhenConsuming_ItemIsRemoved()
        {
            _fries.Use(0, _user);

            _inventoryRepository.Verify(repo => repo.RemoveItem(_user, 0));
        }
    }
}
