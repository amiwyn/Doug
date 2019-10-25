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
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IItemFactory _itemFactory;
        private readonly IGovernmentService _governmentService;
        private readonly ICreditsRepository _creditsRepository;

        public ShopService(IInventoryRepository inventoryRepository, IItemFactory itemFactory, IGovernmentService governmentService, ICreditsRepository creditsRepository)
        {
            _inventoryRepository = inventoryRepository;
            _itemFactory = itemFactory;
            _governmentService = governmentService;
            _creditsRepository = creditsRepository;
        }


        public DougResponse Buy(User user, string itemId)
        {
            var item = _itemFactory.CreateItem(itemId);

            var price = _governmentService.GetPriceWithTaxes(item);

            if (!user.HasEnoughCreditsForAmount(price))
            {
                return new DougResponse(user.NotEnoughCreditsForAmountResponse(price));
            }

            _creditsRepository.RemoveCredits(user.Id, price);

            _inventoryRepository.AddItem(user, item);

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

            if (!item.IsSellable)
            {
                return new DougResponse(DougMessages.ItemNotTradable);
            }

            _inventoryRepository.RemoveItem(user, position);

            _creditsRepository.AddCredits(user.Id, item.Price / 2);

            return new DougResponse(string.Format(DougMessages.SoldItem, item.GetDisplayName(), item.Price / 2));
        }
    }
}
