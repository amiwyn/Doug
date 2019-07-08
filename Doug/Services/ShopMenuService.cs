using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Doug.Items;
using Doug.Menus;
using Doug.Models;
using Doug.Repositories;
using Doug.Slack;

namespace Doug.Services
{
    public interface IShopMenuService
    {
        Task Buy(Interaction interaction);
        Task Sell(Interaction interaction);
    }

    public class ShopMenuService : IShopMenuService
    {
        private readonly IUserRepository _userRepository;
        private readonly ISlackWebApi _slack;
        private readonly IItemFactory _itemFactory;
        private readonly IShopService _shopService;

        public static readonly List<string> ShopItems = new List<string> { ItemFactory.CoffeeCup, ItemFactory.Apple, ItemFactory.Bread, ItemFactory.SteelSword, ItemFactory.ClothArmor }; // TODO: temp. put this in a table somewhere

        public ShopMenuService(IUserRepository userRepository, ISlackWebApi slack, IItemFactory itemFactory, IShopService shopService)
        {
            _userRepository = userRepository;
            _slack = slack;
            _itemFactory = itemFactory;
            _shopService = shopService;
        }

        public async Task Buy(Interaction interaction) 
        {
            var user = _userRepository.GetUser(interaction.UserId);

            var response = _shopService.Buy(user, interaction.Value);

            await _slack.SendEphemeralMessage(response.Message, user.Id, interaction.ChannelId);

            var items = ShopItems.Select(itm => _itemFactory.CreateItem(itm));
            await _slack.UpdateInteractionMessage(new ShopMenu(items, user).Blocks, interaction.ResponseUrl);
        }

        public async Task Sell(Interaction interaction)
        {
            var user = _userRepository.GetUser(interaction.UserId);
            var position = int.Parse(interaction.Value.Split(":").Last());

            var response = _shopService.Sell(user, position);

            await _slack.SendEphemeralMessage(response.Message, user.Id, interaction.ChannelId);

            await _slack.UpdateInteractionMessage(new InventoryMenu(user.InventoryItems).Blocks, interaction.ResponseUrl);
        }
    }
}
