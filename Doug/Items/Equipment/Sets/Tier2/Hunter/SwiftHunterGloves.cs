namespace Doug.Items.Equipment.Sets.Tier2.Hunter
{
    public class SwiftHunterGloves : EquipmentItem
    {
        public const string ItemId = "swift_hunter_gloves";

        public SwiftHunterGloves()
        {
            Id = ItemId;
            Name = "Swift Hunter Gloves";
            Description = "Offer some protection and hitrate.";
            Rarity = Rarity.Common;
            Icon = ":gloves_2:";
            Slot = EquipmentSlot.Gloves;
            Price = 1088;
            LevelRequirement = 25;
            AgilityRequirement = 20;

            Stats.Hitrate = 28;
            Stats.Defense = 10;
        }
    }
}