namespace Doug.Items.Equipment.Sets.Tier2.Mage
{
    public class MageGloves : EquipmentItem
    {
        public const string ItemId = "mage_gloves";

        public MageGloves()
        {
            Id = ItemId;
            Name = "Mage Gloves";
            Description = "Offer some protection and hitrate.";
            Rarity = Rarity.Common;
            Icon = ":gloves_2:";
            Slot = EquipmentSlot.Gloves;
            Price = 882;
            LevelRequirement = 20;
            IntelligenceRequirement = 20;

            Stats.Energy = 60;
            Stats.Hitrate = 12;
            Stats.Defense = 8;
            Stats.EnergyRegen = 2;
        }
    }
}