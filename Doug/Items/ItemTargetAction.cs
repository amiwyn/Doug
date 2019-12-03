using Doug.Models.User;
using Doug.Repositories;

namespace Doug.Items
{
    public abstract class ItemTargetAction
    {
        private readonly IInventoryRepository _inventoryRepository;

        protected ItemTargetAction(IInventoryRepository inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
        }

        public virtual string Activate(int itemPos, User user, User target, string channel)
        {
            _inventoryRepository.RemoveItem(user, itemPos);
            return DougMessages.ConsumedItem;
        }
    }
}
