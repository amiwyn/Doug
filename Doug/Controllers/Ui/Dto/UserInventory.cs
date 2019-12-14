using System.Collections.Generic;
using Doug.Models.User;

namespace Doug.Controllers.Ui.Dto
{
    public class UserInventory
    {
        public List<InventoryItem> Items { get; set; }
        public Loadout Loadout { get; set; }
        public string ActionMessage { get; set; }

        public UserInventory(User user, string actionMessage)
        {
            Items = user.InventoryItems;
            Loadout = user.Loadout;
            ActionMessage = actionMessage;
        }
    }
}
