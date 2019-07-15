namespace Doug.Items.Misc
{
    public class BachelorsDegree : Item
    {
        public const string ItemId = "bachelor_degree";
        public BachelorsDegree()
        {
            Id = ItemId;
            Name = "Bachelor's Degree";
            Description = "Its a sheet of paper with your name on it.";
            Rarity = Rarity.Common;
            Icon = ":contract:";
            Price = 90;
        }
    }
}
