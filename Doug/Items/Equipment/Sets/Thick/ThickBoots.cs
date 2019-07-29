namespace Doug.Items.Equipment.Sets.Thick
{
    public class ThickBoots : EquipmentItem
    {
        public const string ItemId = "thick_boots";

        public ThickBoots()
        {
            Id = ItemId;
            Name = "Thick Boots";
            Description = "These boots are thicc.";
            Rarity = Rarity.Common;
            Icon = ":boots_1:";
            Slot = EquipmentSlot.Boots;
            Price = 260;
            LevelRequirement = 10;
            StrengthRequirement = 15;

            Stats.Defense = 6;
            Stats.Dodge = 5;
        }
    }
}
