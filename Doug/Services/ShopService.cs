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
    public interface IShopService
    {
        Task Buy(Interaction interaction);
        Task Sell(Interaction interaction);
    }

    public class ShopService : IShopService
    {
        private readonly IUserRepository _userRepository;
        private readonly ISlackWebApi _slack;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IItemFactory _itemFactory;

        public static readonly List<string> ShopItems = new List<string> { ItemFactory.CoffeeCup, ItemFactory.Apple, ItemFactory.Bread, ItemFactory.McdoFries }; // TODO: temp. put this in a table somewhere

        public ShopService(IUserRepository userRepository, ISlackWebApi slack, IInventoryRepository inventoryRepository, IItemFactory itemFactory)
        {
            _userRepository = userRepository;
            _slack = slack;
            _inventoryRepository = inventoryRepository;
            _itemFactory = itemFactory;
        }

        public async Task Buy(Interaction interaction)
        {
            var user = _userRepository.GetUser(interaction.UserId);
            var item = _itemFactory.CreateItem(interaction.Value);

            if (!user.HasEnoughCreditsForAmount(item.Price))
            {
                var message = user.NotEnoughCreditsForAmountResponse(item.Price);
                await _slack.SendEphemeralMessage(message, user.Id, interaction.ChannelId);
                return;
            }

            _userRepository.RemoveCredits(user.Id, item.Price);

            _inventoryRepository.AddItem(user, item.Id);

            var items = ShopItems.Select(itm => _itemFactory.CreateItem(itm));

            await _slack.UpdateInteractionMessage(new ShopMenu(items, user).Blocks, interaction.ResponseUrl);
        }

        public async Task Sell(Interaction interaction)
        {
            var user = _userRepository.GetUser(interaction.UserId);
            var position = int.Parse(interaction.Value.Split(":").Last());
            var item = user.InventoryItems.SingleOrDefault(itm => itm.InventoryPosition == position)?.Item;

            if (item == null)
            {
                await _slack.SendEphemeralMessage(string.Format(DougMessages.NoItemInSlot, position), user.Id, interaction.ChannelId);
                return;
            }

            _inventoryRepository.RemoveItem(user, position);

            _userRepository.AddCredits(user.Id, item.Price / 2);

            await _slack.SendEphemeralMessage(string.Format(DougMessages.SoldItem, item.Name, item.Price / 2), user.Id, interaction.ChannelId);

            await _slack.UpdateInteractionMessage(new InventoryMenu(user.InventoryItems).Blocks, interaction.ResponseUrl);
        }
    }
}
