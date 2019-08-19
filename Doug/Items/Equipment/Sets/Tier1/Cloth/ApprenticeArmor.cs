namespace Doug.Items.Equipment.Sets
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
            Price = 669;
            LevelRequirement = 15;
            IntelligenceRequirement = 15;

            Stats.Energy = 60;
            Stats.Defense = 12;
            Stats.Resistance = 8;
            Stats.EnergyRegen = 3;
            Stats.Intelligence = 3;
        }
    }
}