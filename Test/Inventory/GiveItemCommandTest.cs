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
    public class GiveItemCommandTest
    {
        private const string Channel = "coco-channel";
        private const string User = "testuser";

        private User _user;
        private User _target;

        private InventoryService _inventoryService;

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
            var items = new List<InventoryItem>()
            {
                new InventoryItem("testuser", "testitem") {InventoryPosition = 2, Item = new Default()},
                new InventoryItem("testuser", "testitem") {InventoryPosition = 3, Item = new Item()}
            };
            
            _user = new User { Id = "testuser", InventoryItems = items };
            _target = new User { Id = "ginette", InventoryItems = items };

            _userRepository.Setup(repo => repo.GetUser(User)).Returns(_user);
            _userRepository.Setup(repo => repo.GetUser("ginette")).Returns(_target);

            _inventoryService = new InventoryService(_actionFactory.Object, _inventoryRepository.Object, _userService.Object, _slack.Object, _targetActionFactory.Object, _equipmentRepository.Object);
        }

        [TestMethod]
        public void WhenGivingItem_ItemIsRemovedFromGiver()
        {
            _inventoryService.Give(_user, _target, 2, Channel);

            _inventoryRepository.Verify(repo => repo.RemoveItem(_user, 2));
        }

        [TestMethod]
        public void WhenGivingItem_ItemIsAddedToReceiver()
        {
            _inventoryService.Give(_user, _target, 2, Channel);

            _inventoryRepository.Verify(repo => repo.AddItem(_target, It.IsAny<Item>()));
        }

        [TestMethod]
        public void GivenUserHasNoItemInSlot_WhenGivingItem_NoItemMessageSent()
        {
            var user = new User() {Id = "testuser", InventoryItems = new List<InventoryItem>()};

            var result = _inventoryService.Give(user, _target, 2, Channel);

            Assert.AreEqual("There is no item in slot 2.", result);
        }
    }
}
