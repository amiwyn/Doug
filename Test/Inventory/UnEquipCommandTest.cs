using System.Collections.Generic;
using Doug.Commands;
using Doug.Items;
using Doug.Items.Consumables;
using Doug.Items.Equipment;
using Doug.Models;
using Doug.Repositories;
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
        private readonly Mock<IStatsRepository> _statsRepository = new Mock<IStatsRepository>();
        private readonly Mock<IInventoryRepository> _inventoryRepository = new Mock<IInventoryRepository>();
        private readonly Mock<IEquipmentRepository> _equipmentRepository = new Mock<IEquipmentRepository>();

        private EquipmentItem _item;

        [TestInitialize]
        public void Setup()
        {
            _item = new AwakeningOrb(_slack.Object);
            var loadout = new Loadout();
            loadout.Equip(new CloakOfSpikes());

            _userRepository.Setup(repo => repo.GetUser(User)).Returns(new User() { Id = "testuser", Loadout = loadout });
            _equipmentRepository.Setup(repo => repo.UnequipItem("testuser", EquipmentSlot.Body)).Returns(new CloakOfSpikes());

            _inventoryCommands = new InventoryCommands(_userRepository.Object, _slack.Object, _inventoryRepository.Object, _equipmentRepository.Object);
        }

        [TestMethod]
        public void WhenUnEquippingItem_ItemIsRemovedFromSlot()
        {
            _inventoryCommands.UnEquip(_command);

            _equipmentRepository.Verify(repo => repo.UnequipItem(User, EquipmentSlot.Body));
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
