namespace Doug.Items.Equipment.Sets.Thick
{
    public class ThickArmor : EquipmentItem
    {
        public const string ItemId = "thick_armor";

        public ThickArmor()
        {
            Id = ItemId;
            Name = "Thick Armor";
            Description = "An armor made of thicc material.";
            Rarity = Rarity.Common;
            Icon = ":thick_Armor:";
            Slot = EquipmentSlot.Body;
            Price = 387;
            LevelRequirement = 10;
            StrengthRequirement = 15;

            Stats.Defense = 18;
            Stats.Resistance = 7;
        }
    }
}
