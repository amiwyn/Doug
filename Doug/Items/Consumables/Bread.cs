using Doug.Models.User;
using Doug.Repositories;

namespace Doug.Items.Consumables
{
    public class Bread : ConsumableItem
    {
        public const string ItemId = "bread";

        private readonly IStatsRepository _statsRepository;
        private const int RecoverAmount = 50;

        public Bread(IStatsRepository statsRepository, IInventoryRepository inventoryRepository) : base(inventoryRepository)
        {
            _statsRepository = statsRepository;
            Id = ItemId;
            Name = "Bread";
            Description = "A nice steamy loaf of bread. It's fresh from the bakery. Restores 50 health.";
            Rarity = Rarity.Common;
            Icon = ":bread_food:";
            Price = 50;
        }

        public override string Use(int itemPos, User user, string channel)
        {
            base.Use(itemPos, user, channel);

            user.Health += RecoverAmount;

            _statsRepository.UpdateHealth(user.Id, user.Health);

            return string.Format(DougMessages.RecoverItem, Name, RecoverAmount, "health");
        }
    }
}
