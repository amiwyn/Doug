using Doug.Models;
using Doug.Repositories;

namespace Doug.Items.Consumables
{
    public class Apple : ConsumableItem
    {
        public const string ItemId = "apple";

        private readonly IStatsRepository _statsRepository;
        private const int RecoverAmount = 25;

        public Apple() { }

        public Apple(IStatsRepository statsRepository, IInventoryRepository inventoryRepository) : base(inventoryRepository)
        {
            _statsRepository = statsRepository;
            Id = ItemId;
            Name = "Apple";
            Description = "Ahhh, a fresh apple. So healthy. Restores 25 health.";
            Rarity = Rarity.Common;
            Icon = ":apple_food:";
            Price = 25;
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
