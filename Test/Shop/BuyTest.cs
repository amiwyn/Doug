using Doug;
using Doug.Items;
using Doug.Models.User;
using Doug.Repositories;
using Doug.Services;
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
        private readonly Mock<IInventoryRepository>  _inventoryRepository = new Mock<IInventoryRepository>();
        private readonly Mock<IItemRepository> _itemRepository = new Mock<IItemRepository>();
        private readonly Mock<IGovernmentService> _governmentService = new Mock<IGovernmentService>();
        private readonly Mock<ICreditsRepository> _creditsRepository = new Mock<ICreditsRepository>();
        private User _user;

        [TestInitialize]
        public void Setup()
        {
            _governmentService.Setup(repo => repo.GetPriceWithTaxes(It.IsAny<Item>())).Returns((Item item) => item.Price);
            _itemRepository.Setup(factory => factory.GetItem(It.IsAny<string>())).Returns(new Item() { Price = 2727 });
            _user = new User() {Id = "testuser", Credits = 431279};
            _userRepository.Setup(repo => repo.GetUser(User)).Returns(_user);

            _shopService = new ShopService(_inventoryRepository.Object, _itemRepository.Object, _governmentService.Object, _creditsRepository.Object);
        }

        [TestMethod]
        public void GivenEnoughCredits_WhenBuyingAnApple_AppleIsAdded()
        {
            _shopService.Buy(_user, "lucky_dice");

            _inventoryRepository.Verify(repo => repo.AddItem(_user, It.IsAny<Item>()));
        }

        [TestMethod]
        public void GivenEnoughCredits_WhenBuyingAnApple_CreditsAreRemovedFromGiver()
        {
            _shopService.Buy(_user, "lucky_dice");

            _creditsRepository.Verify(repo => repo.RemoveCredits(User, 2727));
        }

        [TestMethod]
        public void GivenNotEnoughCredits_WhenBuyingAnApple_NotEnoughCreditsMessageIsSent()
        {
            var user = new User() {Id = "testuser", Credits = 22};

            var result = _shopService.Buy(user, "lucky_dice");

            Assert.AreEqual("You need 2727 " + DougMessages.CreditEmoji + " to do this and you have 22 " + DougMessages.CreditEmoji, result.Message);
        }
    }
}
