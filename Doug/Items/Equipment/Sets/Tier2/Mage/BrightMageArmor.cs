namespace Doug.Items.Equipment.Sets.Tier2.Mage
{
    public class BrightMageArmor : EquipmentItem
    {
        public const string ItemId = "bright_mage_armor";

        public BrightMageArmor()
        {
            Id = ItemId;
            Name = "Bright Mage Armor";
            Description = "An armor made to protect yourself.";
            Rarity = Rarity.Common;
            Icon = ":mage_coat:";
            Slot = EquipmentSlot.Body;
            Price = 1442;
            LevelRequirement = 25;
            IntelligenceRequirement = 20;

            Stats.Energy = 125;
            Stats.Defense = 16;
            Stats.Resistance = 12;
            Stats.EnergyRegen = 4;
        }
    }
}