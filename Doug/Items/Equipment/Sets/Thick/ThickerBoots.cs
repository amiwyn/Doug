namespace Doug.Items.Equipment.Sets.Thick
{
    public class ThickerBoots : EquipmentItem
    {
        public const string ItemId = "thicker_boots";

        public ThickerBoots()
        {
            Id = ItemId;
            Name = "Thicker Boots";
            Description = "These boots are thicccc.";
            Rarity = Rarity.Common;
            Icon = ":boots_1:";
            Slot = EquipmentSlot.Boots;
            Price = 460;
            LevelRequirement = 15;
            StrengthRequirement = 20;

            Stats.Defense = 11;
            Stats.HitRate = 8;
            Stats.Strength = 1;
        }
    }
}
