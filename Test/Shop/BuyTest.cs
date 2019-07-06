using System.Threading.Tasks;
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
        private const string Channel = "coco-channel";
        private const string User = "testuser";

        private readonly Interaction _interaction = new Interaction
        {
            ChannelId = Channel,
            UserId = User,
            Action = "buy",
            Value = "lucky_dice"
        };

        private ShopMenuService _shopMenuService;

        private readonly Mock<IUserRepository> _userRepository = new Mock<IUserRepository>();
        private readonly Mock<ISlackWebApi> _slack = new Mock<ISlackWebApi>();
        private readonly Mock<IInventoryRepository>  _inventoryRepository = new Mock<IInventoryRepository>();
        private readonly Mock<IItemFactory> _itemFactory = new Mock<IItemFactory>();
        private readonly Mock<IShopService> _shopService = new Mock<IShopService>();
        private User _user;

        [TestInitialize]
        public void Setup()
        {
            _itemFactory.Setup(factory => factory.CreateItem(It.IsAny<string>())).Returns(new LuckyDice());
            _user = new User() {Id = "testuser", Credits = 431279};
            _userRepository.Setup(repo => repo.GetUser(User)).Returns(_user);

            _shopMenuService = new ShopMenuService(_userRepository.Object, _slack.Object, _itemFactory.Object, _shopService.Object);
        }

        [TestMethod]
        public async Task GivenEnoughCredits_WhenBuyingAnApple_AppleIsAdded()
        {
            await _shopMenuService.Buy(_interaction);

            _inventoryRepository.Verify(repo => repo.AddItem(_user, "lucky_dice"));
        }

        [TestMethod]
        public async Task GivenEnoughCredits_WhenBuyingAnApple_CreditsAreRemovedFromGiver()
        {
            await _shopMenuService.Buy(_interaction);

            _userRepository.Verify(repo => repo.RemoveCredits(User, 2674));
        }

        [TestMethod]
        public async Task GivenNotEnoughCredits_WhenBuyingAnApple_NotEnoughCreditsMessageIsSent()
        {
            _userRepository.Setup(repo => repo.GetUser(User)).Returns(new User() { Id = "testuser", Credits = 22 });

            await _shopMenuService.Buy(_interaction);

            _slack.Verify(slack => slack.SendEphemeralMessage(It.IsAny<string>(), User, Channel));
        }
    }
}
