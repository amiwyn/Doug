namespace Doug.Items.Equipment.Sets.Tier2.Mage
{
    public class BrightMageBoots : EquipmentItem
    {
        public const string ItemId = "bright_mage_boots";

        public BrightMageBoots()
        {
            Id = ItemId;
            Name = "Bright Mage Boots";
            Description = "Offer some protection and some dodge.";
            Rarity = Rarity.Common;
            Icon = ":boots_2:";
            Slot = EquipmentSlot.Boots;
            Price = 1115;
            LevelRequirement = 25;
            IntelligenceRequirement = 20;

            Stats.Energy = 60;
            Stats.Dodge = 12;
            Stats.Defense = 12;
        }
    }
}