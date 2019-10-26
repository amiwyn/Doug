using Doug.Models.User;
using Doug.Repositories;

namespace Doug.Items.Consumables.Resets
{
    public class AgilityReset : ConsumableItem
    {
        public const string ItemId = "agi_reset";

        private readonly IStatsRepository _statsRepository;

        public AgilityReset(IStatsRepository statsRepository, IInventoryRepository inventoryRepository) : base(inventoryRepository)
        {
            _statsRepository = statsRepository;
            Id = ItemId;
            Name = "Agility Reset Potion";
            Description = "Reset 1 point of agility.";
            Rarity = Rarity.Rare;
            Icon = ":reset_potion:";
            Price = 25;
        }

        public override string Use(int itemPos, User user, string channel)
        {
            base.Use(itemPos, user, channel);

            _statsRepository.FreeStatPoint(user.Id, Stats.Agility);

            return string.Empty;
        }
    }
}
