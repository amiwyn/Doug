namespace Doug.Models
{
    public class LootItem
    {
        public string Id { get; set; }
        public int Quantity { get; set; }

        public LootItem(string id, int quantity)
        {
            Id = id;
            Quantity = quantity;
        }
    }
}
