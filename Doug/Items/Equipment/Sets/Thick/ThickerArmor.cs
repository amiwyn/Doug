namespace Doug.Items.Equipment.Sets.Thick
{
    public class ThickerArmor : EquipmentItem
    {
        public const string ItemId = "thicker_armor";

        public ThickerArmor()
        {
            Id = ItemId;
            Name = "Thicker Armor";
            Description = "An armor made of thicccc material.";
            Rarity = Rarity.Common;
            Icon = ":thick_Armor:";
            Slot = EquipmentSlot.Body;
            Price = 600;
            LevelRequirement = 15;
            StrengthRequirement = 20;

            Stats.Defense = 22;
            Stats.Resistance = 10;
            Stats.Strength = 2;
            Stats.Constitution = 1;
        }
    }
}
