using Doug.Models;
using Doug.Models.User;

namespace Doug.Items
{
    public class Lootbox : Consumable
    {
        public DropTable DropTable { get; set; }

        public override string Use(IActionFactory actionFactory, int itemPos, User user, string channel)
        {
            base.Use(actionFactory, itemPos, user, channel);
            return actionFactory.OpenLootBox(user, DropTable, channel, GetDisplayName());
        }
    }
}
