using System.Linq;
using System.Threading.Tasks;
using Doug.Controllers;
using Doug.Menus;
using Doug.Models;
using Doug.Models.User;
using Doug.Repositories;
using Doug.Slack;

namespace Doug.Services.MenuServices
{
    public interface IShopMenuService
    {
        Task<DougResponse> ShowShop(User user, string channel, string shopId);
        Task Buy(Interaction interaction);
        Task Sell(Interaction interaction);
        Task ShopSwitch(Interaction interaction);
    }

    public class ShopMenuService : IShopMenuService
    {
        public const string GeneralStoreId = "default";
        public const string PeasantShopId = "peasant";
        public const string ArmoryShopId = "armory";
        public const string RogueShop = "rogue";
        public const string MagicShop = "magic";

        private readonly IUserRepository _userRepository;
        private readonly ISlackWebApi _slack;
        private readonly IItemRepository _itemRepository;
        private readonly IShopService _shopService;
        private readonly IGovernmentService _governmentService;
        private readonly IShopRepository _shopRepository;

        public ShopMenuService(IUserRepository userRepository, ISlackWebApi slack, IItemRepository itemRepository, IShopService shopService, IGovernmentService governmentService, IShopRepository shopRepository)
        {
            _userRepository = userRepository;
            _slack = slack;
            _itemRepository = itemRepository;
            _shopService = shopService;
            _governmentService = governmentService;
            _shopRepository = shopRepository;
        }

        public async Task<DougResponse> ShowShop(User user, string channel, string shopId)
        {
            Shop shop;
            if (shopId == null)
            {
                shop = _shopRepository.GetShop(channel) ?? _shopRepository.GetShop(GeneralStoreId);
            }
            else
            {
                shop = _shopRepository.GetShop(shopId);
            }

            if (shop == null)
            {
                return new DougResponse(DougMessages.UnknownShop);
            }

            await _slack.SendEphemeralBlocks(new ShopMenu(shop, user, _itemRepository, _governmentService).Blocks, user.Id, channel);

            return new DougResponse();
        }

        public async Task Buy(Interaction interaction) 
        {
            var user = _userRepository.GetUser(interaction.UserId);
            var shopId = interaction.BlockId.Split(":").First();
            var shop = _shopRepository.GetShop(shopId);

            var response = _shopService.Buy(user, interaction.Value);

            await _slack.SendEphemeralMessage(response.Message, user.Id, interaction.ChannelId);

            await _slack.UpdateInteractionMessage(new ShopMenu(shop, user, _itemRepository, _governmentService).Blocks, interaction.ResponseUrl);
        }

        public async Task Sell(Interaction interaction)
        {
            var user = _userRepository.GetUser(interaction.UserId);
            var position = int.Parse(interaction.Value.Split(":").Last());

            var response = _shopService.Sell(user, position);

            await _slack.SendEphemeralMessage(response.Message, user.Id, interaction.ChannelId);

            await _slack.UpdateInteractionMessage(new InventoryMenu(user.InventoryItems).Blocks, interaction.ResponseUrl);
        }

        public async Task ShopSwitch(Interaction interaction)
        {
            var user = _userRepository.GetUser(interaction.UserId);
            var shop = _shopRepository.GetShop(interaction.Value);

            await _slack.UpdateInteractionMessage(new ShopMenu(shop, user, _itemRepository, _governmentService).Blocks, interaction.ResponseUrl);
        }
    }
}
