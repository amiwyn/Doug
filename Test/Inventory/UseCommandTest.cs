using System.Collections.Generic;
using Doug.Commands;
using Doug.Items;
using Doug.Models;
using Doug.Repositories;
using Doug.Slack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Test.Inventory
{
    [TestClass]
    public class UseCommandTest
    {
        private const string CommandText = "2";
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
        private readonly Mock<IInventoryRepository> _inventoryRepository = new Mock<IInventoryRepository>();
        private readonly Mock<IEquipmentRepository> _equipmentRepository = new Mock<IEquipmentRepository>();
        private readonly Mock<ISlackWebApi> _slack = new Mock<ISlackWebApi>();
        private readonly Mock<Item> _item = new Mock<Item>();

        [TestInitialize]
        public void Setup()
        {
            var items = new List<InventoryItem>() {new InventoryItem("testuser", "testitem") {InventoryPosition = 2, Item = _item.Object } };
            _userRepository.Setup(repo => repo.GetUser(User)).Returns(new User() { Id = "testuser", InventoryItems = items });

            _inventoryCommands = new InventoryCommands(_userRepository.Object, _slack.Object, _inventoryRepository.Object, _equipmentRepository.Object);
        }

        [TestMethod]
        public void WhenUsingItem_ItemIsRemovedFromGiver()
        {
            _inventoryCommands.Use(_command);

            _item.Verify(item => item.Use(2, It.IsAny<User>(), Channel));
        }

        [TestMethod]
        public void GivenUserHasNoItemInSlot_WhenGivingItem_NoItemMessageSent()
        {
            _userRepository.Setup(repo => repo.GetUser(User)).Returns(new User() { Id = "testuser", InventoryItems = new List<InventoryItem>() });

            var result = _inventoryCommands.Use(_command);

            Assert.AreEqual("There is no item in slot 2.", result.Message);
        }
    }
}
