using System.Linq;
using Doug.Items;
using Doug.Models;
using Doug.Repositories;
using Doug.Slack;

namespace Doug.Services
{
    public interface IShopService
    {
        void Buy(Interaction interaction);
        void Sell(Interaction interaction);
    }

    public class ShopService : IShopService
    {
        private readonly IUserRepository _userRepository;
        private readonly ISlackWebApi _slack;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IItemFactory _itemFactory;

        public ShopService(IUserRepository userRepository, ISlackWebApi slack, IInventoryRepository inventoryRepository, IItemFactory itemFactory)
        {
            _userRepository = userRepository;
            _slack = slack;
            _inventoryRepository = inventoryRepository;
            _itemFactory = itemFactory;
        }

        public void Buy(Interaction interaction)
        {
            var user = _userRepository.GetUser(interaction.UserId);
            var item = _itemFactory.CreateItem(interaction.Value);

            if (!user.HasEnoughCreditsForAmount(item.Price))
            {
                var message = user.NotEnoughCreditsForAmountResponse(item.Price);
                _slack.SendEphemeralMessage(message, user.Id, interaction.ChannelId);
                return;
            }

            _userRepository.RemoveCredits(user.Id, item.Price);

            _inventoryRepository.AddItem(user, item.Id);
        }

        public void Sell(Interaction interaction)
        {
            var user = _userRepository.GetUser(interaction.UserId);
            var position = int.Parse(interaction.Value.Split(":").Last());
            var item = user.InventoryItems.SingleOrDefault(itm => itm.InventoryPosition == position)?.Item;

            if (item == null)
            {
                _slack.SendEphemeralMessage(string.Format(DougMessages.NoItemInSlot, position), user.Id, interaction.ChannelId);
                return;
            }

            _inventoryRepository.RemoveItem(user, position);

            _userRepository.AddCredits(user.Id, item.Price / 2);

            _slack.SendEphemeralMessage(string.Format(DougMessages.SoldItem, item.Name, item.Price / 2), user.Id, interaction.ChannelId);
        }
    }
}
