namespace Doug.Models
{
    public class ShopItem
    {
        public string ShopId { get; set; }
        public string ItemId { get; set; }
        public Shop Shop { get; set; }
    }
}
