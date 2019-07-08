using Doug.Models;
using Doug.Repositories;

namespace Doug.Items.Consumables.Resets
{
    public class CharismaReset : ConsumableItem
    {
        private readonly IStatsRepository _statsRepository;

        public CharismaReset(IStatsRepository statsRepository, IInventoryRepository inventoryRepository) : base(inventoryRepository)
        {
            _statsRepository = statsRepository;
            Id = ItemFactory.CharismaReset;
            Name = "Charisma Reset Potion";
            Description = "Reset 1 point of charisma.";
            Rarity = Rarity.Rare;
            Icon = ":tropical_drink:";
            Price = 25;
        }

        public override string Use(int itemPos, User user, string channel)
        {
            base.Use(itemPos, user, channel);

            _statsRepository.FreeStatPoint(user.Id, Stats.Charisma);

            return string.Empty;
        }
    }
}
