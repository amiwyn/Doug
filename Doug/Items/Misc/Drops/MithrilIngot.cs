namespace Doug.Items.Misc.Drops
{
    public class MithrilIngot : Item
    {
        public const string ItemId = "mithril_ingot";
        public MithrilIngot()
        {
            Id = ItemId;
            Name = "Mithril Ingot";
            Description = "An ingot made of mithril metal.";
            Rarity = Rarity.Uncommon;
            Icon = ":mithril_ingot:";
            Price = 2880;
            MaxStack = 99;
        }
    }
}
