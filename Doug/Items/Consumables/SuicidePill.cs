using Doug.Models.User;
using Doug.Repositories;
using Doug.Services;

namespace Doug.Items.Consumables
{
    public class SuicidePill : ConsumableItem
    {
        public const string ItemId = "suicide_pill";

        private readonly IUserService _userService;

        public SuicidePill(IInventoryRepository inventoryRepository, IUserService userService) : base(inventoryRepository)
        {
            _userService = userService;
            Id = ItemId;
            Name = "Suicide Pill";
            Description = "When you want to end it all...";
            Rarity = Rarity.Common;
            Icon = ":pill1:";
            Price = 10;
        }

        public override string Use(int itemPos, User user, string channel)
        {
            _userService.KillUser(user, channel);

            return base.Use(itemPos, user, channel);
        }
    }
}
