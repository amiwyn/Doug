using Doug.Models;
using Doug.Repositories;

namespace Doug.Items.Consumables.Resets
{
    public class LuckReset : ConsumableItem
    {
        public const string ItemId = "luck_reset";

        private readonly IStatsRepository _statsRepository;

        public LuckReset(IStatsRepository statsRepository, IInventoryRepository inventoryRepository) : base(inventoryRepository)
        {
            _statsRepository = statsRepository;
            Id = ItemId;
            Name = "Luck Reset Potion";
            Description = "Reset 1 point of luck.";
            Rarity = Rarity.Rare;
            Icon = ":reset_potion:";
            Price = 25;
        }

        public override string Use(int itemPos, User user, string channel)
        {
            base.Use(itemPos, user, channel);

            _statsRepository.FreeStatPoint(user.Id, Stats.Luck);

            return string.Empty;
        }
    }
}
