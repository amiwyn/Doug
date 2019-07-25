using Doug.Models;
using Doug.Repositories;

namespace Doug.Items.Consumables.Resets
{
    public class IntelligenceReset : ConsumableItem
    {
        public const string ItemId = "int_reset";

        private readonly IStatsRepository _statsRepository;

        public IntelligenceReset(IStatsRepository statsRepository, IInventoryRepository inventoryRepository) : base(inventoryRepository)
        {
            _statsRepository = statsRepository;
            Id = ItemId;
            Name = "Intelligence Reset Potion";
            Description = "Reset 1 point of intelligence.";
            Rarity = Rarity.Rare;
            Icon = ":reset_potion:";
            Price = 25;
        }

        public override string Use(int itemPos, User user, string channel)
        {
            base.Use(itemPos, user, channel);

            _statsRepository.FreeStatPoint(user.Id, Stats.Intelligence);

            return string.Empty;
        }
    }
}
