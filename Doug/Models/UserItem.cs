using Doug.Items;

namespace Doug.Models
{
    public class UserItem
    {
        public string UserId { get; set; }
        public User User { get; set; }
        public int ItemId { get; set; }
        public Item Item { get; set; }
        public int InventoryId { get; set; }
    }
}
