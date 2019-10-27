using Doug.Models.User;
using Doug.Repositories;
using Doug.Services;

namespace Doug.Items.Consumables
{
    public class McdoFries : ConsumableItem
    {
        public const string ItemId = "mcdo_fries";

        private readonly IStatsRepository _statsRepository;
        private readonly IUserService _userService;
        private const int RecoverAmount = 50;
        private const int LossAmount = 25;

        public McdoFries(IStatsRepository statsRepository, IInventoryRepository inventoryRepository, IUserService userService) : base(inventoryRepository)
        {
            _statsRepository = statsRepository;
            _userService = userService;
            Id = ItemId;
            Name = "Mc Donald fries";
            Description = "Whats this? Salty stale fried potato sticks?! One must be crazy to even consider buying those. Restores 50 energy but lose 25 health.";
            Rarity = Rarity.Uncommon;
            Icon = ":fritesmcdo:";
            Price = 50;
        }

        public override string Use(int itemPos, User user, string channel)
        {
            base.Use(itemPos, user, channel);

            user.Energy += RecoverAmount;

            _statsRepository.UpdateEnergy(user.Id, user.Energy);

            _userService.ApplyTrueDamage(user, LossAmount, channel);

            return string.Format(DougMessages.RecoverItem, Name, RecoverAmount, "energy");
        }
    }
}
