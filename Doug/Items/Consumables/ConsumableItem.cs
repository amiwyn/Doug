using Doug.Models;
using Doug.Repositories;

namespace Doug.Items.Consumables
{
    public abstract class ConsumableItem : Item
    {
        protected ConsumableItem()
        {
            MaxStack = 99;
        }

        public override string Use(int itemPos, User user, IUserRepository userRepository, IStatsRepository statsRepository)
        {
            userRepository.RemoveItem(user.Id, itemPos);
            return DougMessages.ConsumedItem;
        }
    }
}
