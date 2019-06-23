using Doug.Models;
using Doug.Repositories;

namespace Doug.Items
{
    public abstract class ConsumableItem : Item
    {
        protected ConsumableItem()
        {
            MaxStack = 99;
        }

        public override string Use(int itemPos, User user, IInventoryRepository inventoryRepository, IStatsRepository statsRepository)
        {
            inventoryRepository.RemoveItem(user.Id, itemPos);
            return DougMessages.ConsumedItem;
        }
    }
}
