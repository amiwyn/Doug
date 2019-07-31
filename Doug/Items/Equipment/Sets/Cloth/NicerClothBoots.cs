namespace Doug.Items.Equipment.Sets.Cloth
{
    public class NicerClothBoots : EquipmentItem
    {
        public const string ItemId = "nicer_cloth_boots";

        public ClothBoots()
        {
            Id = ItemId;
            Name = "Nicer Cloth Boots";
            Description = "These boots are made of nicer cloth.";
            Rarity = Rarity.Common;
            Icon = ":boots_1:";
            Slot = EquipmentSlot.Boots;
            Price = 460;
            LevelRequirement = 15;
            IntelligenceRequirement = 20;

            Stats.Defense = 10;
            Stats.Dodge = 8;
            Stats.Energy = 30;
        }
    }
}
