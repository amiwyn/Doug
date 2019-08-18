namespace Doug.Items.Equipment.Sets.Tier1.Cloth
{
    public class ApprenticeArmor : EquipmentItem
    {
        public const string ItemId = "apprentice_armor";

        public ApprenticeArmor()
        {
            Id = ItemId;
            Name = "Apprentice Armor";
            Description = "An armor made of cloth.";
            Rarity = Rarity.Common;
            Icon = ":cloth_armor:";
            Slot = EquipmentSlot.Body;
            Price = 787;
            LevelRequirement = 15;
            IntelligenceRequirement = 20;

            Stats.Defense = 15;
            Stats.Resistance = 8;
            Stats.Energy = 40;
            Stats.Intelligence = 3;
            Stats.EnergyRegen = 2;
        }
    }
}
