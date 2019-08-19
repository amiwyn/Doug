namespace Doug.Items.Equipment.Sets.Tier2.Hunter
{
    public class HunterBoots : EquipmentItem
    {
        public const string ItemId = "hunter_boots";

        public HunterBoots()
        {
            Id = ItemId;
            Name = "Hunter Boots";
            Description = "Offer some protection and some dodge.";
            Rarity = Rarity.Common;
            Icon = ":boots_2:";
            Slot = EquipmentSlot.Boots;
            Price = 824;
            LevelRequirement = 20;
            AgilityRequirement = 20;

            Stats.Dodge = 18;
            Stats.Defense = 10;
        }
    }
}