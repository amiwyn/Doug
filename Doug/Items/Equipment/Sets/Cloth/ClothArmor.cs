namespace Doug.Items.Equipment.Sets.Cloth
{
    public class ClothArmor : EquipmentItem
    {
        public const string ItemId = "cloth_armor";

        public ClothArmor()
        {
            Id = ItemId;
            Name = "Cloth Armor";
            Description = "An armor made of cloth.";
            Rarity = Rarity.Common;
            Icon = ":cloth_armor:";
            Slot = EquipmentSlot.Body;
            Price = 387;
            LevelRequirement = 10;
            IntelligenceRequirement = 15;

            Stats.Defense = 10;
            Stats.Resistance = 7;
            Stats.Energy = 40;
            Stats.Intelligence = 4;
        }
    }
}
