namespace Doug.Items.Equipment.Sets.Cloth
{
    public class NicerClothArmor : EquipmentItem
    {
        public const string ItemId = "nicer_cloth_armor";

        public ClothArmor()
        {
            Id = ItemId;
            Name = "Nicer Cloth Armor";
            Description = "An armor made of nicer cloth.";
            Rarity = Rarity.Common;
            Icon = ":cloth_armor:";
            Slot = EquipmentSlot.Body;
            Price = 547;
            LevelRequirement = 15;
            IntelligenceRequirement = 20;

            Stats.Defense = 13;
            Stats.Resistance = 9;
            Stats.Energy = 60;
            Stats.Intelligence = 6;
            Stats.Constitution = 1;
        }
    }
}
