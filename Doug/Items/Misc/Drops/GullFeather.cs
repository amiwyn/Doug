namespace Doug.Items.Misc.Drops
{
    public class GullFeather : Item
    {
        public const string ItemId = "gull_feather";
        public GullFeather()
        {
            Id = ItemId;
            Name = "Gull Feather";
            Description = "Its a pristine white feather.";
            Rarity = Rarity.Common;
            Icon = ":feather:";
            Price = 153;
        }
    }
}
