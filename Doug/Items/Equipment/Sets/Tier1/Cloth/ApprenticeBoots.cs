namespace Doug.Items.Equipment.Sets
{
    public class ApprenticeBoots : EquipmentItem
    {
        public const string ItemId = "apprentice_boots";

        public ApprenticeBoots()
        {
            Id = ItemId;
            Name = "Apprentice Boots";
            Description = "These boots are made of cloth.";
            Rarity = Rarity.Common;
            Icon = ":boots_1:";
            Slot = EquipmentSlot.Boots;
            Price = 484;
            LevelRequirement = 15;
            IntelligenceRequirement = 15;

            Stats.Energy = 30;
            Stats.Dodge = 8;
            Stats.Defense = 8;
        }
    }
}