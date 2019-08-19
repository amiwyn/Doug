namespace Doug.Items.Equipment.Sets
{
    public class ToughPlateBoots : EquipmentItem
    {
        public const string ItemId = "tough_boots";

        public ToughPlateBoots()
        {
            Id = ItemId;
            Name = "Tough Plate Boots";
            Description = "These boots are thicc.";
            Rarity = Rarity.Common;
            Icon = ":boots_1:";
            Slot = EquipmentSlot.Boots;
            Price = 484;
            LevelRequirement = 15;
            StrengthRequirement = 15;

            Stats.Dodge = 7;
            Stats.Defense = 8;
        }
    }
}