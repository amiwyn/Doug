using Doug.Models;
using Doug.Repositories;

namespace Doug.Items.Consumables
{
    public class CoffeeCup : ConsumableItem
    {
        private readonly IStatsRepository _statsRepository;
        private const int RecoverAmount = 25;

        public CoffeeCup(IStatsRepository statsRepository, IInventoryRepository inventoryRepository) : base(inventoryRepository)
        {
            _statsRepository = statsRepository;
            Id = ItemFactory.CoffeeCup;
            Name = "Coffee";
            Description = "A good cuppa. Restores 25 energy.";
            Rarity = Rarity.Common;
            Icon = ":coffee_bowl:";
            Price = 50;
        }

        public override string Use(int itemPos, User user, string channel)
        {
            base.Use(itemPos, user, channel);

            user.Energy += RecoverAmount;

            _statsRepository.UpdateEnergy(user.Id, user.Energy);

            return string.Format(DougMessages.RecoverItem, Name, RecoverAmount, "energy");
        }
    }
}
