using Doug;
using Doug.Items;
using Doug.Items.Equipment;
using Doug.Models;
using Doug.Repositories;
using Doug.Services;
using Doug.Slack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Test.Shop
{
    [TestClass]
    public class BuyTest
    {
        private const string User = "testuser";

        private ShopService _shopService;

        private readonly Mock<IUserRepository> _userRepository = new Mock<IUserRepository>();
        private readonly Mock<ISlackWebApi> _slack = new Mock<ISlackWebApi>();
        private readonly Mock<IInventoryRepository>  _inventoryRepository = new Mock<IInventoryRepository>();
        private readonly Mock<IItemFactory> _itemFactory = new Mock<IItemFactory>();
        private User _user;

        [TestInitialize]
        public void Setup()
        {
            _itemFactory.Setup(factory => factory.CreateItem(It.IsAny<string>())).Returns(new LuckyDice());
            _user = new User() {Id = "testuser", Credits = 431279};
            _userRepository.Setup(repo => repo.GetUser(User)).Returns(_user);

            _shopService = new ShopService(_userRepository.Object, _inventoryRepository.Object, _itemFactory.Object);
        }

        [TestMethod]
        public void GivenEnoughCredits_WhenBuyingAnApple_AppleIsAdded()
        {
            _shopService.Buy(_user, "lucky_dice");

            _inventoryRepository.Verify(repo => repo.AddItem(_user, "lucky_dice"));
        }

        [TestMethod]
        public void GivenEnoughCredits_WhenBuyingAnApple_CreditsAreRemovedFromGiver()
        {
            _shopService.Buy(_user, "lucky_dice");

            _userRepository.Verify(repo => repo.RemoveCredits(User, 2674));
        }

        [TestMethod]
        public void GivenNotEnoughCredits_WhenBuyingAnApple_NotEnoughCreditsMessageIsSent()
        {
            var user = new User() {Id = "testuser", Credits = 22};

            var result = _shopService.Buy(user, "lucky_dice");

            Assert.AreEqual("You need 2674 " + DougMessages.CreditEmoji + " to do this and you have 22 " + DougMessages.CreditEmoji, result.Message);
        }
    }
}
