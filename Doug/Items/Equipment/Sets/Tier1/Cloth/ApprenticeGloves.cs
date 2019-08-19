namespace Doug.Items.Equipment.Sets.Tier1.Cloth
{
    public class ApprenticeGloves : EquipmentItem
    {
        public const string ItemId = "apprentice_gloves";

        public ApprenticeGloves()
        {
            Id = ItemId;
            Name = "Apprentice Gloves";
            Description = "Some cloth gloves.";
            Rarity = Rarity.Common;
            Icon = ":gloves_1:";
            Slot = EquipmentSlot.Gloves;
            Price = 484;
            LevelRequirement = 15;
            IntelligenceRequirement = 15;

            Stats.Energy = 30;
            Stats.Hitrate = 10;
            Stats.Defense = 6;
            Stats.EnergyRegen = 2;
            Stats.Intelligence = 1;
        }
    }
}