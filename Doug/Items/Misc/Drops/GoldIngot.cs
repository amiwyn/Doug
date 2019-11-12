namespace Doug.Items.Misc.Drops
{
    public class GoldIngot : Item
    {
        public const string ItemId = "gold_ingot";
        public GoldIngot()
        {
            Id = ItemId;
            Name = "Gold Ingot";
            Description = "An ingot made of gold metal.";
            Rarity = Rarity.Uncommon;
            Icon = ":gold_ingot:";
            Price = 1696;
            MaxStack = 99;
        }
    }
}
