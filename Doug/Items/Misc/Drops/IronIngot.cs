namespace Doug.Items.Misc.Drops
{
    public class IronIngot : Item
    {
        public const string ItemId = "iron_ingot";
        public IronIngot()
        {
            Id = ItemId;
            Name = "Iron Ingot";
            Description = "An ingot made of iron.";
            Rarity = Rarity.Common;
            Icon = ":iron_ingot:";
            Price = 430;
        }
    }
}
