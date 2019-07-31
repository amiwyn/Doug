namespace Doug.Items.Misc.Drops
{
    public class BikerCocaine : Item
    {
        public const string ItemId = "biker_coca";
        public BikerCocaine()
        {
            Id = ItemId;
            Name = "Biker's Cocaine";
            Description = "Half glass, half cocaine, half the price.";
            Rarity = Rarity.Common;
            Icon = ":powder:";
            Price = 100;
            MaxStack = 99;
        }
    }
}
