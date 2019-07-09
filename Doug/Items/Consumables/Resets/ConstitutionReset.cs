using Doug.Models;
using Doug.Repositories;

namespace Doug.Items.Consumables.Resets
{
    public class ConstitutionReset : ConsumableItem
    {
        private readonly IStatsRepository _statsRepository;

        public ConstitutionReset(IStatsRepository statsRepository, IInventoryRepository inventoryRepository) : base(inventoryRepository)
        {
            _statsRepository = statsRepository;
            Id = ItemFactory.ConstitutionReset;
            Name = "Constitution Reset Potion";
            Description = "Reset 1 point of constitution.";
            Rarity = Rarity.Rare;
            Icon = ":tropical_drink:";
            Price = 25;
        }

        public override string Use(int itemPos, User user, string channel)
        {
            base.Use(itemPos, user, channel);

            _statsRepository.FreeStatPoint(user.Id, Stats.Constitution);

            return string.Empty;
        }
    }
}
