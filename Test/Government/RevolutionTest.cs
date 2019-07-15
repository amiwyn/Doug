using System.Collections.Generic;
using Doug.Items;
using Doug.Items.Equipment;
using Doug.Models;
using Doug.Repositories;
using Doug.Services;
using Doug.Slack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Test.Government
{
    [TestClass]
    public class RevolutionTest
    {
        private const string Channel = "coco-channel";

        private GovernmentService _governmentService;
        private User _oldRuler; 
        private User _newRuler; 

        private readonly Mock<IUserRepository> _userRepository = new Mock<IUserRepository>();
        private readonly Mock<ISlackWebApi> _slack = new Mock<ISlackWebApi>();
        private readonly Mock<IEquipmentRepository> _equipmentRepository = new Mock<IEquipmentRepository>();
        private readonly Mock<IInventoryRepository> _inventoryRepository = new Mock<IInventoryRepository>();
        private readonly Mock<IUserService> _userService = new Mock<IUserService>();
        private readonly Mock<IGovernmentRepository> _governmentRepository = new Mock<IGovernmentRepository>();

        [TestInitialize]
        public void Setup()
        {
            _governmentRepository.Setup(repo => repo.GetGovernment()).Returns(new Doug.Models.Government { Ruler = "wgf", RevolutionLeader = "gab"});
            _oldRuler = new User {Loadout = new Loadout() {Head = "crown"}};
            _newRuler = new User();

            _userRepository.Setup(repo => repo.GetUser("wgf")).Returns(_oldRuler);
            _userRepository.Setup(repo => repo.GetUser("gab")).Returns(_newRuler);

            _governmentService = new GovernmentService(_governmentRepository.Object, _slack.Object, _userService.Object, _userRepository.Object, _equipmentRepository.Object, _inventoryRepository.Object);
        }

        [TestMethod]
        public void WhenRevolting_RulerIsChanged()
        {
            _governmentService.Revolution(Channel);

            _governmentService.Revolution(Channel);
        }

        [TestMethod]
        public void WhenRevolting_OldRulerIsKilled()
        {
            _governmentService.Revolution(Channel);

            _userService.Verify(service => service.ApplyMagicalDamage(It.IsAny<User>(), 69696969, Channel));
        }

        [TestMethod]
        public void GivenRulerHasCrownEquipped_WhenRevolting_RulersCrownIsRemoved()
        {
            _oldRuler = new User { Loadout = new Loadout() { Head = "crown" } };
            _userRepository.Setup(repo => repo.GetUser("wgf")).Returns(_oldRuler);

            _governmentService.Revolution(Channel);

            _equipmentRepository.Verify(repo => repo.UnequipItem(_oldRuler, EquipmentSlot.Head));
        }

        [TestMethod]
        public void GivenRulerHasCrownNotEquipped_WhenRevolting_RulersCrownIsRemoved()
        {
            var inventory = new List<InventoryItem> { new InventoryItem("wgf", "crown") { Item = new Crown() } };
            _oldRuler = new User {Loadout = new Loadout(), InventoryItems = inventory};
            _userRepository.Setup(repo => repo.GetUser("wgf")).Returns(_oldRuler);

            _governmentService.Revolution(Channel);

            _inventoryRepository.Verify(repo => repo.RemoveItem(_oldRuler, 0));
        }

        [TestMethod]
        public void WhenRevolting_NewRulerReceiveCrown()
        {
            _governmentService.Revolution(Channel);

            _inventoryRepository.Verify(repo => repo.AddItem(_newRuler, "crown"));
        }
    }
}
