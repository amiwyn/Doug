using System.Linq;
using Doug.Items;
using Doug.Models;
using Doug.Repositories;

namespace Doug.Services
{
    public interface IShopService
    {
        DougResponse Buy(User user, string itemId);
        DougResponse Sell(User user, int position);
    }

    public class ShopService : IShopService
    {
        private readonly IUserRepository _userRepository;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IItemFactory _itemFactory;

        public ShopService(IUserRepository userRepository, IInventoryRepository inventoryRepository, IItemFactory itemFactory)
        {
            _userRepository = userRepository;
            _inventoryRepository = inventoryRepository;
            _itemFactory = itemFactory;
        }


        public DougResponse Buy(User user, string itemId)
        {
            var item = _itemFactory.CreateItem(itemId);

            if (!user.HasEnoughCreditsForAmount(item.Price))
            {
                return new DougResponse(user.NotEnoughCreditsForAmountResponse(item.Price));
            }

            _userRepository.RemoveCredits(user.Id, item.Price);

            _inventoryRepository.AddItem(user, item.Id);

            return new DougResponse();
        }

        public DougResponse Sell(User user, int position)
        {
            var item = user.InventoryItems.SingleOrDefault(itm => itm.InventoryPosition == position)?.Item;

            if (item == null)
            {
                return new DougResponse(string.Format(DougMessages.NoItemInSlot, position));
            }

            if (!item.IsTradable)
            {
                return new DougResponse(DougMessages.ItemNotTradable);
            }

            _inventoryRepository.RemoveItem(user, position);

            _userRepository.AddCredits(user.Id, item.Price / 2);

            return new DougResponse(string.Format(DougMessages.SoldItem, item.Name, item.Price / 2));
        }
    }
}
