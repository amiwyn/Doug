using Doug.Items;

namespace Doug.Models.Monsters
{
    public class LootItem
    {
        public string Id { get; set; }
        public int Quantity { get; set; }
        public Item Item { get; set; }

        public LootItem(string id, int quantity, Item item)
        {
            Id = id;
            Quantity = quantity;
            Item = item;
        }

        public LootItem(Item item, int quantity)
        {
            Quantity = quantity;
            Item = item;
        }
    }
}
