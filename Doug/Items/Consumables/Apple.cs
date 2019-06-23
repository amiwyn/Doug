using Doug.Models;
using Doug.Repositories;

namespace Doug.Items.Consumables
{
    public class Apple : ConsumableItem
    {
        private const int RecoverAmount = 25;

        public Apple()
        {
            Name = "Apple";
            Description = "Ahhh, a fresh apple. So healthy. Restore 25 health.";
            Rarity = Rarity.Common;
            Icon = ":apple:";
        }

        public override string Use(int itemPos, User user, IInventoryRepository inventoryRepository, IStatsRepository statsRepository)
        {
            base.Use(itemPos, user, inventoryRepository, statsRepository);

            user.Health += RecoverAmount;

            statsRepository.UpdateHealth(user.Id, user.Health);

            return string.Format(DougMessages.RecoverItem, Name, RecoverAmount, "health");
        }
    }
}
