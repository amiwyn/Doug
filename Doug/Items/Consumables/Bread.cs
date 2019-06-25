using Doug.Models;
using Doug.Repositories;

namespace Doug.Items.Consumables
{
    public class Bread : ConsumableItem
    {
        private readonly IStatsRepository _statsRepository;
        private const int RecoverAmount = 50;

        public Bread(IStatsRepository statsRepository, IInventoryRepository inventoryRepository) : base(inventoryRepository)
        {
            _statsRepository = statsRepository;
            Id = ItemFactory.Bread;
            Name = "Bread";
            Description = "A nice steamy loaf of bread. It's fresh from the bakery. Restores 25 health.";
            Rarity = Rarity.Common;
            Icon = ":bread:";
            Price = 50;
        }

        public override string Use(int itemPos, User user)
        {
            base.Use(itemPos, user);

            user.Health += RecoverAmount;

            _statsRepository.UpdateHealth(user.Id, user.Health);

            return string.Format(DougMessages.RecoverItem, Name, RecoverAmount, "health");
        }
    }
}
