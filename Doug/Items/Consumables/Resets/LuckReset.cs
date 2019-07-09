using Doug.Models;
using Doug.Repositories;

namespace Doug.Items.Consumables.Resets
{
    public class LuckReset : ConsumableItem
    {
        private readonly IStatsRepository _statsRepository;

        public LuckReset(IStatsRepository statsRepository, IInventoryRepository inventoryRepository) : base(inventoryRepository)
        {
            _statsRepository = statsRepository;
            Id = ItemFactory.LuckReset;
            Name = "Luck Reset Potion";
            Description = "Reset 1 point of luck.";
            Rarity = Rarity.Rare;
            Icon = ":tropical_drink:";
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
