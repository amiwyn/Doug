using System.Collections.Generic;
using Doug.Commands;
using Doug.Items;
using Doug.Items.Misc;
using Doug.Items.Tickets;
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
    public class GiveItemCommandTest
    {
        private const string CommandText = "<@ginette|username> 2";
        private const string Channel = "coco-channel";
        private const string User = "testuser";

        private readonly Command _command = new Command()
        {
            ChannelId = Channel,
            Text = CommandText,
            UserId = User
        };

        private User _user;
        private User _target;

        private InventoryCommands _inventoryCommands;

        private readonly Mock<IUserRepository> _userRepository = new Mock<IUserRepository>();
        private readonly Mock<ISlackWebApi> _slack = new Mock<ISlackWebApi>();
        private readonly Mock<IInventoryRepository> _inventoryRepository = new Mock<IInventoryRepository>();
        private readonly Mock<IEquipmentRepository> _equipmentRepository = new Mock<IEquipmentRepository>();
        private readonly Mock<IUserService> _userService = new Mock<IUserService>();

        [TestInitialize]
        public void Setup()
        {
            var items = new List<InventoryItem>()
            {
                new InventoryItem("testuser", "testitem") {InventoryPosition = 2, Item = new Default()},
                new InventoryItem("testuser", "testitem") {InventoryPosition = 3, Item = new KickTicket(null, null, null, null)}
            };
            
            _user = new User() { Id = "testuser", InventoryItems = items };
            _target = new User() { Id = "ginette", InventoryItems = items };

            _userRepository.Setup(repo => repo.GetUser(User)).Returns(_user);
            _userRepository.Setup(repo => repo.GetUser("ginette")).Returns(_target);

            _inventoryCommands = new InventoryCommands(_userRepository.Object, _slack.Object, _inventoryRepository.Object, _equipmentRepository.Object, _userService.Object);
        }

        [TestMethod]
        public void WhenGivingItem_ItemIsRemovedFromGiver()
        {
            _inventoryCommands.Give(_command);

            _inventoryRepository.Verify(repo => repo.RemoveItem(_user, 2));
        }

        [TestMethod]
        public void WhenGivingItem_ItemIsAddedToReceiver()
        {
            _inventoryCommands.Give(_command);

            _inventoryRepository.Verify(repo => repo.AddItem(_target, It.IsAny<Item>()));
        }

        [TestMethod]
        public void GivenUserHasNoItemInSlot_WhenGivingItem_NoItemMessageSent()
        {
            _userRepository.Setup(repo => repo.GetUser(User)).Returns(new User() { Id = "testuser", InventoryItems = new List<InventoryItem>() });

            var result = _inventoryCommands.Give(_command);

            Assert.AreEqual("There is no item in slot 2.", result.Message);
        }

        //[TestMethod]
        //public void GivenItemIsNotTradable_WhenGivingItem_ItemNotTradableMessage()
        //{
        //    var command = new Command()
        //    {
        //        ChannelId = Channel,
        //        Text = "<@ginette|username> 3",
        //        UserId = User
        //    };

        //    var result = _inventoryCommands.Give(command);

        //    Assert.AreEqual("This item is not tradable.", result.Message);
        //}
    }
}
