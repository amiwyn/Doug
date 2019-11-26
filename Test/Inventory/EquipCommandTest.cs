using System.Collections.Generic;
using Doug.Commands;
using Doug.Items;
using Doug.Models;
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
        private const string CommandText = "6";
        private const string Channel = "coco-channel";
        private const string User = "testuser";

        private readonly Command _command = new Command()
        {
            ChannelId = Channel,
            Text = CommandText,
            UserId = User
        };

        private InventoryCommands _inventoryCommands;

        private readonly Mock<IUserRepository> _userRepository = new Mock<IUserRepository>();
        private readonly Mock<ISlackWebApi> _slack = new Mock<ISlackWebApi>();
        private readonly Mock<IInventoryRepository> _inventoryRepository = new Mock<IInventoryRepository>();
        private readonly Mock<IEquipmentRepository> _equipmentRepository = new Mock<IEquipmentRepository>();
        private readonly Mock<IUserService> _userService = new Mock<IUserService>();
        private readonly Mock<IActionFactory> _actionFactory = new Mock<IActionFactory>();
        private readonly Mock<ITargetActionFactory> _targetActionFactory = new Mock<ITargetActionFactory>();

        private EquipmentItem _item;

        [TestInitialize]
        public void Setup()
        {
            _item = new EquipmentItem();
            var loadout = new Loadout();
            var items = new List<InventoryItem>() { new InventoryItem("testuser", "testitem") { InventoryPosition = 6, Item = _item } };
            _userRepository.Setup(repo => repo.GetUser(User)).Returns(new User { Id = "testuser", Experience = 35000, InventoryItems = items, Loadout = loadout });
            _equipmentRepository.Setup(repo => repo.EquipItem(It.IsAny<User>(), It.IsAny<EquipmentItem>())).Returns(new List<EquipmentItem>());

            _inventoryCommands = new InventoryCommands(_userRepository.Object, _slack.Object, _inventoryRepository.Object, _equipmentRepository.Object, _userService.Object, _actionFactory.Object, _targetActionFactory.Object);
        }

        [TestMethod]
        public void GivenEmptyEquipmentSlot_WhenEquippingItem_ItemIsEquippedInSlot()
        {
            _inventoryCommands.Equip(_command);

            _equipmentRepository.Verify(repo => repo.EquipItem(It.IsAny<User>(), _item));
        }

        [TestMethod]
        public void GivenEmptyEquipmentSlot_WhenEquippingItem_ItemIsRemovedFromInventory()
        {
            _inventoryCommands.Equip(_command);

            _inventoryRepository.Verify(repo => repo.RemoveItem(It.IsAny<User>(), 6));
        }

        [TestMethod]
        public void GivenUserHasNoItemInSlot_WhenEquipping_NoItemMessageSent()
        {
            _userRepository.Setup(repo => repo.GetUser(User)).Returns(new User() { Id = "testuser", InventoryItems = new List<InventoryItem>() });

            var result = _inventoryCommands.Equip(_command);

            Assert.AreEqual("There is no item in slot 6.", result.Message);
        }

        [TestMethod]
        public void GivenItemIsNotEquipAble_WhenEquipping_NotEquipAbleMessageSent()
        {
            var items = new List<InventoryItem>() { new InventoryItem("testuser", "testitem") { InventoryPosition = 6, Item = new Consumable() } };
            _userRepository.Setup(repo => repo.GetUser(User)).Returns(new User() { Id = "testuser", InventoryItems = items });

            var result = _inventoryCommands.Equip(_command);

            Assert.AreEqual("This item is not equipable.", result.Message);
        }

        [TestMethod]
        public void GivenItemLevelIsTooHigh_WhenEquipping_ItemLevelTooHighMessageSent()
        {
            var items = new List<InventoryItem>() { new InventoryItem("testuser", "testitem") { InventoryPosition = 6, Item = new EquipmentItem { LevelRequirement = 69 } } };
            _userRepository.Setup(repo => repo.GetUser(User)).Returns(new User() { Id = "testuser", InventoryItems = items });

            var result = _inventoryCommands.Equip(_command);

            Assert.AreEqual("You do not meet the level requirements to equip this item.", result.Message);
        }
    }
}
