using System.Collections.Generic;
using Doug.Commands;
using Doug.Items;
using Doug.Items.Equipment;
using Doug.Models;
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
        private const string CommandText = "1";
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

        [TestInitialize]
        public void Setup()
        {
            var loadout = new Loadout();
            loadout.Equip(new CloakOfSpikes());

            _userRepository.Setup(repo => repo.GetUser(User)).Returns(new User() { Id = "testuser", Loadout = loadout });
            _equipmentRepository.Setup(repo => repo.UnequipItem(It.IsAny<User>(), EquipmentSlot.Body)).Returns(new CloakOfSpikes());

            _inventoryCommands = new InventoryCommands(_userRepository.Object, _slack.Object, _inventoryRepository.Object, _equipmentRepository.Object, _userService.Object);
        }

        [TestMethod]
        public void WhenUnEquippingItem_ItemIsRemovedFromSlot()
        {
            _inventoryCommands.UnEquip(_command);

            _equipmentRepository.Verify(repo => repo.UnequipItem(It.IsAny<User>(), EquipmentSlot.Body));
        }

        [TestMethod]
        public void WhenUnEquippingItem_ItemIsAddedToInventory()
        {
            _inventoryCommands.UnEquip(_command);

            _inventoryRepository.Verify(repo => repo.AddItem(It.IsAny<User>(), ItemFactory.CloakOfSpikes));
        }

        [TestMethod]
        public void GivenUserHasNoItemInSlot_WhenUnEquippingItem_NoItemMessageSent()
        {
            _userRepository.Setup(repo => repo.GetUser(User)).Returns(new User() { Id = "testuser", InventoryItems = new List<InventoryItem>() });

            var result = _inventoryCommands.UnEquip(_command);

            Assert.AreEqual("No equipment in slot 1", result.Message);
        }
    }
}
