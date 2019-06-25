using Doug.Models;
using Doug.Repositories;

namespace Doug.Items.Consumables
{
    public class McdoFries : ConsumableItem
    {
        private readonly IStatsRepository _statsRepository;
        private const int RecoverAmount = 50;
        private const int LossAmount = 25;

        public McdoFries(IStatsRepository statsRepository, IInventoryRepository inventoryRepository) : base(inventoryRepository)
        {
            _statsRepository = statsRepository;
            Id = ItemFactory.McdoFries;
            Name = "Mc Donald fries";
            Description = "Whats this? Salty stale fried potato sticks?! One must be crazy to even consider buying those. Restores 50 energy but lose 25 health.";
            Rarity = Rarity.Uncommon;
            Icon = ":fritesmcdo:";
            Price = 50;
        }

        public override string Use(int itemPos, User user)
        {
            base.Use(itemPos, user);

            user.Health -= LossAmount;
            user.Energy += RecoverAmount;

            _statsRepository.UpdateHealth(user.Id, user.Health);
            _statsRepository.UpdateEnergy(user.Id, user.Energy);

            return string.Format(DougMessages.RecoverItem, Name, RecoverAmount, "energy");
        }
    }
}
