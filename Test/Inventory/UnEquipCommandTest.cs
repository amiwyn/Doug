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
    public class UnEquipCommandTest
    {
        private const string User = "testuser";

        private InventoryService _inventoryService;
        private User _user;

        private readonly Mock<IUserRepository> _userRepository = new Mock<IUserRepository>();
        private readonly Mock<ISlackWebApi> _slack = new Mock<ISlackWebApi>();
        private readonly Mock<IInventoryRepository> _inventoryRepository = new Mock<IInventoryRepository>();
        private readonly Mock<IEquipmentRepository> _equipmentRepository = new Mock<IEquipmentRepository>();
        private readonly Mock<IUserService> _userService = new Mock<IUserService>();
        private readonly Mock<IActionFactory> _actionFactory = new Mock<IActionFactory>();
        private readonly Mock<ITargetActionFactory> _targetActionFactory = new Mock<ITargetActionFactory>();

        [TestInitialize]
        public void Setup()
        {
            var loadout = new Loadout();
            loadout.Equip(new EquipmentItem() { Slot = EquipmentSlot.Body});

            _user = new User() {Id = "testuser", Loadout = loadout};

            _userRepository.Setup(repo => repo.GetUser(User)).Returns(_user);
            _equipmentRepository.Setup(repo => repo.UnequipItem(It.IsAny<User>(), EquipmentSlot.Body)).Returns(new EquipmentItem());

            _inventoryService = new InventoryService(_actionFactory.Object, _inventoryRepository.Object, _userService.Object, _slack.Object, _targetActionFactory.Object, _equipmentRepository.Object);
        }

        [TestMethod]
        public void WhenUnEquippingItem_ItemIsRemovedFromSlot()
        {
            _inventoryService.UnEquip(_user, EquipmentSlot.Body);

            _equipmentRepository.Verify(repo => repo.UnequipItem(It.IsAny<User>(), EquipmentSlot.Body));
        }

        [TestMethod]
        public void WhenUnEquippingItem_ItemIsAddedToInventory()
        {
            _inventoryService.UnEquip(_user, EquipmentSlot.Body);

            _inventoryRepository.Verify(repo => repo.AddItem(It.IsAny<User>(), It.IsAny<EquipmentItem>()));
        }

        [TestMethod]
        public void GivenUserHasNoItemInSlot_WhenUnEquippingItem_NoItemMessageSent()
        {
            var user = new User() {Id = "testuser", InventoryItems = new List<InventoryItem>()};
            var result = _inventoryService.UnEquip(user, EquipmentSlot.Body);

            Assert.AreEqual("No equipment in slot Body", result);
        }
    }
}
