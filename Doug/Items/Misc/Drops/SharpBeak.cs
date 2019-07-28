namespace Doug.Items.Misc.Drops
{
    public class SharpBeak : Item
    {
        public const string ItemId = "sharp_beak";
        public SharpBeak()
        {
            Id = ItemId;
            Name = "Sharp Beak";
            Description = "Its razor sharp";
            Rarity = Rarity.Common;
            Icon = ":beak:";
            Price = 134;
        }
    }
}
