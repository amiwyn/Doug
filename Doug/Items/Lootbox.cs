using Doug.Models;
using Doug.Models.User;

namespace Doug.Items
{
    public class Lootbox : Consumable
    {
        public string DropTableId { get; set; }
        public DropTable DropTable { get; set; }

        public override string Use(IActionFactory actionFactory, int itemPos, User user, string channel)
        {
            base.Use(actionFactory, itemPos, user, channel);
            return actionFactory.OpenLootBox(itemPos, user, DropTable, channel, GetDisplayName());
        }
    }
}
