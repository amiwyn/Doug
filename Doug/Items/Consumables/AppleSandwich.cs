using Doug.Models.User;
using Doug.Repositories;

namespace Doug.Items.Consumables
{
    public class AppleSandwich : ConsumableItem
    {
        public const string ItemId = "apple_sandwich";

        private readonly IStatsRepository _statsRepository;
        private const int RecoverAmount = 100;

        public AppleSandwich(IStatsRepository statsRepository, IInventoryRepository inventoryRepository) : base(inventoryRepository)
        {
            _statsRepository = statsRepository;
            Id = ItemId;
            Name = "Apple chausson";
            Description = "Incredible, you made this. I didn't know you could cook.";
            Rarity = Rarity.Common;
            Icon = ":square_sandwich:";
            Price = 70 - 1;
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
