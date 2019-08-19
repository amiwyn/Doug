namespace Doug.Items.Equipment.Sets.Tier2.Mage
{
    public class MagePlume : EquipmentItem
    {
        public const string ItemId = "mage_hat";

        public MagePlume()
        {
            Id = ItemId;
            Name = "Mage Plume";
            Description = "To look more feminine.";
            Rarity = Rarity.Common;
            Icon = ":mage_plume_hat:";
            Slot = EquipmentSlot.Head;
            Price = 712;
            LevelRequirement = 20;
            IntelligenceRequirement = 20;

            Stats.Energy = 80;
            Stats.Defense = 4;
            Stats.EnergyRegen = 1;
            Stats.Intelligence = 2;
        }
    }
}