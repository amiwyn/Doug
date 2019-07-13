using Doug.Models;
using Doug.Repositories;

namespace Doug.Items.Consumables.Resets
{
    public class StaminaReset : ConsumableItem
    {
        private readonly IStatsRepository _statsRepository;

        public StaminaReset(IStatsRepository statsRepository, IInventoryRepository inventoryRepository) : base(inventoryRepository)
        {
            _statsRepository = statsRepository;
            Id = ItemFactory.StaminaReset;
            Name = "Stamina Reset Potion";
            Description = "Reset 1 point of stamina.";
            Rarity = Rarity.Rare;
            Icon = ":reset_potion:";
            Price = 25;
        }

        public override string Use(int itemPos, User user, string channel)
        {
            base.Use(itemPos, user, channel);

            _statsRepository.FreeStatPoint(user.Id, Stats.Stamina);

            return string.Empty;
        }
    }
}
