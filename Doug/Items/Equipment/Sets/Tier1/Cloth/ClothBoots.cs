namespace Doug.Items.Equipment.Sets.Tier1.Cloth
{
    public class ClothBoots : EquipmentItem
    {
        public const string ItemId = "cloth_boots";

        public ClothBoots()
        {
            Id = ItemId;
            Name = "Cloth Boots";
            Description = "These boots are made of cloth.";
            Rarity = Rarity.Common;
            Icon = ":boots_1:";
            Slot = EquipmentSlot.Boots;
            Price = 260;
            LevelRequirement = 10;
            IntelligenceRequirement = 15;

            Stats.Defense = 6;
            Stats.Dodge = 6;
            Stats.Energy = 20;
        }
    }
}
