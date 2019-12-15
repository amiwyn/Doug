using System.Collections.Generic;
using Doug.Items;
using Doug.Models.User;
using Doug.Repositories;
using Doug.Services;
using Doug.Slack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Test.Inventory
{
    [TestClass]
    public class EquipCommandTest
    {
        private const string User = "testuser";

        private InventoryService _inventoryService;

        private readonly Mock<IUserRepository> _userRepository = new Mock<IUserRepository>();
        private readonly Mock<ISlackWebApi> _slack = new Mock<ISlackWebApi>();
        private readonly Mock<IInventoryRepository> _inventoryRepository = new Mock<IInventoryRepository>();
        private readonly Mock<IEquipmentRepository> _equipmentRepository = new Mock<IEquipmentRepository>();
        private readonly Mock<IUserService> _userService = new Mock<IUserService>();
        private readonly Mock<IActionFactory> _actionFactory = new Mock<IActionFactory>();
        private readonly Mock<ITargetActionFactory> _targetActionFactory = new Mock<ITargetActionFactory>();

        private EquipmentItem _item;

        private User _user;

        [TestInitialize]
        public void Setup()
        {
            _item = new EquipmentItem();
            var loadout = new Loadout();

            var items = new List<InventoryItem>() { new InventoryItem("testuser", "testitem") { InventoryPosition = 6, Item = _item } };
            _user = new User {Id = "testuser", Experience = 35000, InventoryItems = items, Loadout = loadout};

            _userRepository.Setup(repo => repo.GetUser(User)).Returns(_user);
            _equipmentRepository.Setup(repo => repo.EquipItem(It.IsAny<User>(), It.IsAny<EquipmentItem>())).Returns(new List<EquipmentItem>());

            _inventoryService = new InventoryService(_actionFactory.Object, _inventoryRepository.Object, _userService.Object, _slack.Object, _targetActionFactory.Object, _equipmentRepository.Object);
        }

        [TestMethod]
        public void GivenEmptyEquipmentSlot_WhenEquippingItem_ItemIsEquippedInSlot()
        {
            _inventoryService.Equip(_user, 6);

            _equipmentRepository.Verify(repo => repo.EquipItem(It.IsAny<User>(), _item));
        }

        [TestMethod]
        public void GivenEmptyEquipmentSlot_WhenEquippingItem_ItemIsRemovedFromInventory()
        {
            _inventoryService.Equip(_user, 6);

            _inventoryRepository.Verify(repo => repo.RemoveItem(It.IsAny<User>(), 6));
        }

        [TestMethod]
        public void GivenUserHasNoItemInSlot_WhenEquipping_NoItemMessageSent()
        {
            var result = _inventoryService.Equip(new User { Id = "testuser", InventoryItems = new List<InventoryItem>() }, 6);

            Assert.AreEqual("There is no item in slot 6.", result);
        }

        [TestMethod]
        public void GivenItemIsNotEquipAble_WhenEquipping_NotEquipAbleMessageSent()
        {
            var items = new List<InventoryItem> { new InventoryItem("testuser", "testitem") { InventoryPosition = 6, Item = new Consumable() } };

            var result = _inventoryService.Equip(new User { Id = "testuser", InventoryItems = items }, 6);

            Assert.AreEqual("This item is not equipable.", result);
        }

        [TestMethod]
        public void GivenItemLevelIsTooHigh_WhenEquipping_ItemLevelTooHighMessageSent()
        {
            var items = new List<InventoryItem> { new InventoryItem("testuser", "testitem") { InventoryPosition = 6, Item = new EquipmentItem { LevelRequirement = 69 } } };

            var result = _inventoryService.Equip(new User { Id = "testuser", InventoryItems = items }, 6);

            Assert.AreEqual("You do not meet the level requirements to equip this item.", result);
        }
    }
}
