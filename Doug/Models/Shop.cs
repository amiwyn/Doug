using System.Collections.Generic;

namespace Doug.Models
{
    public class Shop
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double PriceMultiplier { get; set; }
        public List<ShopItem> ShopItems { get; set; }
    }
}
