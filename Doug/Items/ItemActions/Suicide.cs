using Doug.Models.User;
using Doug.Repositories;
using Doug.Services;

namespace Doug.Items.ItemActions
{
    public class Suicide : ItemAction
    {
        private readonly IUserService _userService;

        public Suicide(IInventoryRepository inventoryRepository, IUserService userService) : base(inventoryRepository)
        {
            _userService = userService;
        }

        public override string Activate(int itemPos, User user, string channel)
        {
            _userService.KillUser(user, channel);

            return DougMessages.Cleansed;
        }
    }
}
