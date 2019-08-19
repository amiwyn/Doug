namespace Doug.Items.Equipment.Sets.Tier2.Mage
{
    public class BrightMageGloves : EquipmentItem
    {
        public const string ItemId = "bright_mage_gloves";

        public BrightMageGloves()
        {
            Id = ItemId;
            Name = "Bright Mage Gloves";
            Description = "Offer some protection and hitrate.";
            Rarity = Rarity.Common;
            Icon = ":gloves_2:";
            Slot = EquipmentSlot.Gloves;
            Price = 1088;
            LevelRequirement = 25;
            IntelligenceRequirement = 20;

            Stats.Energy = 70;
            Stats.Hitrate = 14;
            Stats.Defense = 10;
            Stats.EnergyRegen = 3;
        }
    }
}