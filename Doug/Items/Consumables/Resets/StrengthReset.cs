using Doug.Models;
using Doug.Repositories;

namespace Doug.Items.Consumables.Resets
{
    public class StrengthReset : ConsumableItem
    {
        private readonly IStatsRepository _statsRepository;

        public StrengthReset(IStatsRepository statsRepository, IInventoryRepository inventoryRepository) : base(inventoryRepository)
        {
            _statsRepository = statsRepository;
            Id = ItemFactory.StrengthReset;
            Name = "Strength Reset Potion";
            Description = "Reset 1 point of strength.";
            Rarity = Rarity.Rare;
            Icon = ":tropical_drink:";
            Price = 25;
        }

        public override string Use(int itemPos, User user, string channel)
        {
            base.Use(itemPos, user, channel);

            _statsRepository.FreeStatPoint(user.Id, Stats.Strength);

            return string.Empty;
        }
    }
}
