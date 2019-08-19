namespace Doug.Items.Equipment.Sets.Tier2.Hunter
{
    public class SwiftHunterBoots : EquipmentItem
    {
        public const string ItemId = "swift_hunter_boots";

        public SwiftHunterBoots()
        {
            Id = ItemId;
            Name = "Swift Hunter Boots";
            Description = "Offer some protection and some dodge.";
            Rarity = Rarity.Common;
            Icon = ":boots_2:";
            Slot = EquipmentSlot.Boots;
            Price = 1115;
            LevelRequirement = 25;
            AgilityRequirement = 20;

            Stats.Dodge = 20;
            Stats.Defense = 12;
            Stats.Luck = 3;
        }
    }
}