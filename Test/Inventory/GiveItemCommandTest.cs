using System.Collections.Generic;
using Doug.Commands;
using Doug.Models;
using Doug.Repositories;
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

        private InventoryCommands _inventoryCommands;

        private readonly Mock<IUserRepository> _userRepository = new Mock<IUserRepository>();
        private readonly Mock<ISlackWebApi> _slack = new Mock<ISlackWebApi>();
        private readonly Mock<IStatsRepository> _statsRepository = new Mock<IStatsRepository>();
        private readonly Mock<IInventoryRepository> _inventoryRepository = new Mock<IInventoryRepository>();

        [TestInitialize]
        public void Setup()
        {
            var items = new List<InventoryItem>() {new InventoryItem("testuser", "testitem") {InventoryPosition = 2}};
            _userRepository.Setup(repo => repo.GetUser(User)).Returns(new User() { Id = "testuser", InventoryItems = items });

            _inventoryCommands = new InventoryCommands(_userRepository.Object, _slack.Object, _statsRepository.Object, _inventoryRepository.Object);
        }

        [TestMethod]
        public void WhenGivingItem_ItemIsRemovedFromGiver()
        {
            _inventoryCommands.Give(_command);

            _inventoryRepository.Verify(repo => repo.RemoveItem(User, 2));
        }

        [TestMethod]
        public void WhenGivingItem_ItemIsAddedToReceiver()
        {
            _inventoryCommands.Give(_command);

            _inventoryRepository.Verify(repo => repo.AddItem("ginette", "testitem"));
        }

        [TestMethod]
        public void GivenUserHasNoItemInSlot_WhenGivingItem_NoItemMessageSent()
        {
            _userRepository.Setup(repo => repo.GetUser(User)).Returns(new User() { Id = "testuser", InventoryItems = new List<InventoryItem>() });

            var result = _inventoryCommands.Give(_command);

            Assert.AreEqual("There is no item in slot 2.", result.Message);
        }
    }
}
