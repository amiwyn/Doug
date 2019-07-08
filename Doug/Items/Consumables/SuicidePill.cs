using Doug.Effects;
using Doug.Models;
using Doug.Repositories;
using Doug.Services;

namespace Doug.Items.Consumables
{
    public class SuicidePill : ConsumableItem
    {
        private readonly IUserService _userService;
        private const int LossAmount = 69696969;

        public SuicidePill(IInventoryRepository inventoryRepository, IUserService userService) : base(inventoryRepository)
        {
            _userService = userService;
            Id = ItemFactory.SuicidePill;
            Name = "Suicide Pill";
            Description = "When you want to end it all...";
            Rarity = Rarity.Common;
            Icon = ":pill:";
            Price = 10;
        }

        public override string Use(int itemPos, User user, string channel)
        {
            _userService.RemoveHealth(user, LossAmount, channel);

            return base.Use(itemPos, user, channel);
        }
    }
}
