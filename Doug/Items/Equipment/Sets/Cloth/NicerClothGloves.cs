namespace Doug.Items.Equipment.Sets.Cloth
{
    public class NicerClothGloves : EquipmentItem
    {
        public const string ItemId = "nicer_cloth_gloves";

        public NicerClothGloves()
        {
            Id = ItemId;
            Name = "Nicer Cloth Gloves";
            Description = "Some nicer cloth gloves.";
            Rarity = Rarity.Common;
            Icon = ":gloves_1:";
            Slot = EquipmentSlot.Gloves;
            Price = 450;
            LevelRequirement = 15;
            IntelligenceRequirement = 20;

            Stats.Defense = 7;
            Stats.Intelligence = 3;
            Stats.Energy = 30;
            Stats.Constitution = 1
        }
    }
}
