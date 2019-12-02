using Doug.Models.User;
using Doug.Repositories;

namespace Doug.Items
{
    public abstract class ItemAction
    {
        private readonly IInventoryRepository _inventoryRepository;

        protected ItemAction(IInventoryRepository inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
        }

        public virtual string Activate(int itemPos, User user, string channel)
        {
            _inventoryRepository.RemoveItem(user, itemPos);
            return DougMessages.ConsumedItem;
        }
    }
}
