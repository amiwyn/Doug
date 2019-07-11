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
        private readonly IGovernmentService _governmentService;

        public ShopService(IUserRepository userRepository, IInventoryRepository inventoryRepository, IItemFactory itemFactory, IGovernmentService governmentService)
        {
            _userRepository = userRepository;
            _inventoryRepository = inventoryRepository;
            _itemFactory = itemFactory;
            _governmentService = governmentService;
        }


        public DougResponse Buy(User user, string itemId)
        {
            var item = _itemFactory.CreateItem(itemId);

            var price = _governmentService.GetPriceWithTaxes(item);

            if (!user.HasEnoughCreditsForAmount(price))
            {
                return new DougResponse(user.NotEnoughCreditsForAmountResponse(price));
            }

            _userRepository.RemoveCredits(user.Id, price);

            _inventoryRepository.AddItem(user, item.Id);

            _governmentService.CollectSalesTaxes(item);

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
