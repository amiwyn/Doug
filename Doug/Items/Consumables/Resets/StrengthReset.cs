using Doug.Models.User;
using Doug.Repositories;

namespace Doug.Items.Consumables.Resets
{
    public class StrengthReset : ConsumableItem
    {
        public const string ItemId = "str_reset";

        private readonly IStatsRepository _statsRepository;

        public StrengthReset(IStatsRepository statsRepository, IInventoryRepository inventoryRepository) : base(inventoryRepository)
        {
            _statsRepository = statsRepository;
            Id = ItemId;
            Name = "Strength Reset Potion";
            Description = "Reset 1 point of strength.";
            Rarity = Rarity.Rare;
            Icon = ":reset_potion:";
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
