namespace Doug.Items.Misc.Drops
{
    public class SilverIngot : Item
    {
        public const string ItemId = "silver_ingot";
        public SilverIngot()
        {
            Id = ItemId;
            Name = "Silver Ingot";
            Description = "An ingot made of silver metal.";
            Rarity = Rarity.Common;
            Icon = ":silver_ingot:";
            Price = 860;
            MaxStack = 99;
        }
    }
}
