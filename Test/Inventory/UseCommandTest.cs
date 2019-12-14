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
    public class UseCommandTest
    {
        private const string Channel = "coco-channel";
        private const string User = "testuser";

        private InventoryService _inventoryService;
        private User _user;

        private readonly Mock<IUserRepository> _userRepository = new Mock<IUserRepository>();
        private readonly Mock<IInventoryRepository> _inventoryRepository = new Mock<IInventoryRepository>();
        private readonly Mock<IEquipmentRepository> _equipmentRepository = new Mock<IEquipmentRepository>();
        private readonly Mock<IUserService> _userService = new Mock<IUserService>();
        private readonly Mock<ISlackWebApi> _slack = new Mock<ISlackWebApi>();
        private readonly Mock<Item> _item = new Mock<Item>();
        private readonly Mock<IActionFactory> _actionFactory = new Mock<IActionFactory>();
        private readonly Mock<ITargetActionFactory> _targetActionFactory = new Mock<ITargetActionFactory>();

        [TestInitialize]
        public void Setup()
        {
            var items = new List<InventoryItem> {new InventoryItem("testuser", "testitem") {InventoryPosition = 2, Item = _item.Object } };
            _user = new User {Id = "testuser", InventoryItems = items};


            _userRepository.Setup(repo => repo.GetUser(User)).Returns(_user);

            _inventoryService = new InventoryService(_actionFactory.Object, _inventoryRepository.Object, _userService.Object, _slack.Object, _targetActionFactory.Object, _equipmentRepository.Object);
        }

        [TestMethod]
        public void WhenUsingItem_ItemIsRemovedFromGiver()
        {
            _inventoryService.Use(_user, 2, Channel);

            _item.Verify(item => item.Use(_actionFactory.Object, 2, It.IsAny<User>(), Channel));
        }

        [TestMethod]
        public void GivenUserHasNoItemInSlot_WhenGivingItem_NoItemMessageSent()
        {
            var user = new User() {Id = "testuser", InventoryItems = new List<InventoryItem>()};
            var result = _inventoryService.Use(user, 2, Channel);

            Assert.AreEqual("There is no item in slot 2.", result);
        }
    }
}
