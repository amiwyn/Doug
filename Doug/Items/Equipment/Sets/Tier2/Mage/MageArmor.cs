namespace Doug.Items.Equipment.Sets.Tier2.Mage
{
    public class MageArmor : EquipmentItem
    {
        public const string ItemId = "mage_armor";

        public MageArmor()
        {
            Id = ItemId;
            Name = "Mage Armor";
            Description = "An armor made to protect yourself.";
            Rarity = Rarity.Common;
            Icon = ":mage_coat:";
            Slot = EquipmentSlot.Body;
            Price = 1024;
            LevelRequirement = 20;
            IntelligenceRequirement = 20;

            Stats.Energy = 100;
            Stats.Defense = 14;
            Stats.Resistance = 10;
            Stats.EnergyRegen = 4;
        }
    }
}