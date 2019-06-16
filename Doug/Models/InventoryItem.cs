using System.ComponentModel.DataAnnotations.Schema;
using Doug.Items;

namespace Doug.Models
{
    public class InventoryItem
    {
        public string UserId { get; set; }
        public User User { get; set; }
        public string ItemId { get; set; }
        [NotMapped]
        public Item Item { get; set; }
        public int InventoryPosition { get; set; }

        public InventoryItem(string userId, string itemId)
        {
            UserId = userId;
            ItemId = itemId;

            Item = ItemFactory.CreateItem(itemId);
        }
    }
}
