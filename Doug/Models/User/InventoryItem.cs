using Doug.Items;

namespace Doug.Models.User
{
    public class InventoryItem
    {
        public string ItemId { get; set; }
        public Item Item { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public int InventoryPosition { get; set; }
        public int Quantity { get; set; }

        public InventoryItem(string userId, string itemId)
        {
            UserId = userId;
            ItemId = itemId;
        }

        public void CreateItemEffects(IEquipmentEffectFactory equipmentEffectFactory)
        {
            if (Item is EquipmentItem equipmentItem)
            {
                equipmentItem.CreateEffect(equipmentEffectFactory);
            }
        }
    }
}
