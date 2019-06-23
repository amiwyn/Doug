using Doug.Models;
using Doug.Repositories;

namespace Doug.Items.Consumables
{
    public class NormalEnergyDrink : ConsumableItem
    {
        private const int RecoverAmount = 25;

        public NormalEnergyDrink()
        {
            Id = ItemFactory.NormalEnergyDrink;
            Name = "Coffee";
            Description = "A good cuppa. Restore 25 energy.";
            Rarity = Rarity.Common;
            Icon = ":coffee:";
        }

        public override string Use(int itemPos, User user, IInventoryRepository inventoryRepository, IStatsRepository statsRepository)
        {
            base.Use(itemPos, user, inventoryRepository, statsRepository);

            user.Energy += RecoverAmount;

            statsRepository.UpdateEnergy(user.Id, user.Energy);

            return string.Format(DougMessages.RecoverItem, Name, RecoverAmount, "energy");
        }
    }
}
