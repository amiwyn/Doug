namespace Doug.Models
{
    public class LootItem
    {
        public string Id { get; set; }
        public int Quantity { get; set; }
        public double Probability { get; set; }

        public LootItem(string id, int quantity, double probability)
        {
            Id = id;
            Quantity = quantity;
            Probability = probability;
        }
    }
}
