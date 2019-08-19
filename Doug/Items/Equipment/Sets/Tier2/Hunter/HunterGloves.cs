namespace Doug.Items.Equipment.Sets.Tier2.Hunter
{
    public class HunterGloves : EquipmentItem
    {
        public const string ItemId = "hunter_gloves";

        public HunterGloves()
        {
            Id = ItemId;
            Name = "Hunter Gloves";
            Description = "Offer some protection and hitrate.";
            Rarity = Rarity.Common;
            Icon = ":gloves_2:";
            Slot = EquipmentSlot.Gloves;
            Price = 882;
            LevelRequirement = 20;
            AgilityRequirement = 20;

            Stats.Hitrate = 24;
            Stats.Defense = 8;
        }
    }
}