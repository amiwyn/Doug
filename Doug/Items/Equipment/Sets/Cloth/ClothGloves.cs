namespace Doug.Items.Equipment.Sets.Cloth
{
    public class ClothGloves : EquipmentItem
    {
        public const string ItemId = "cloth_gloves";

        public ClothGloves()
        {
            Id = ItemId;
            Name = "Cloth Gloves";
            Description = "Some cloth gloves.";
            Rarity = Rarity.Common;
            Icon = ":gloves_1:";
            Slot = EquipmentSlot.Gloves;
            Price = 250;
            LevelRequirement = 10;
            IntelligenceRequirement = 15;

            Stats.Defense = 4;
            Stats.Intelligence = 2;
            Stats.Energy = 20;
        }
    }
}
