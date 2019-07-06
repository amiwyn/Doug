using System.Collections.Generic;
using Doug.Items;
using Doug.Items.Equipment;
using Doug.Models;
using Doug.Repositories;
using Doug.Services;
using Doug.Slack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Test.Shop
{
    [TestClass]
    public class SellTest
    {
        private const string Channel = "coco-channel";
        private const string User = "testuser";

        private ShopService _shopService;

        private readonly Mock<IUserRepository> _userRepository = new Mock<IUserRepository>();
        private readonly Mock<ISlackWebApi> _slack = new Mock<ISlackWebApi>();
        private readonly Mock<IInventoryRepository>  _inventoryRepository = new Mock<IInventoryRepository>();
        private readonly Mock<IItemFactory> _itemFactory = new Mock<IItemFactory>();
        private User _user;

        [TestInitialize]
        public void Setup()
        {
            var items = new List<InventoryItem>() { new InventoryItem("testuser", "testitem") { InventoryPosition = 4, Item = new LuckyDice() } };
            _user = new User() { Id = "testuser", InventoryItems = items };
            _userRepository.Setup(repo => repo.GetUser(User)).Returns(_user);

            _shopService = new ShopService(_userRepository.Object, _inventoryRepository.Object, _itemFactory.Object);
        }

        [TestMethod]
        public void WhenSelling_MoneyIsAdded()
        {
            _shopService.Sell(_user, 4);

            _userRepository.Verify(repo => repo.AddCredits(User, 1337));
        }

        [TestMethod]
        public void WhenSelling_ItemIsRemoved()
        {
            _shopService.Sell(_user, 4);

            _inventoryRepository.Verify(repo => repo.RemoveItem(_user, 4));
        }

        [TestMethod]
        public void GivenUserHasNoItemInSlot_WhenSelling_NoItemMessageSent()
        {
            var user = new User { Id = "testuser", InventoryItems = new List<InventoryItem>() };

            var result = _shopService.Sell(user, 4);

            Assert.AreEqual("There is no item in slot 4.", result.Message);
        }
    }
}
