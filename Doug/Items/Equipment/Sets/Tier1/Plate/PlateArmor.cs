namespace Doug.Items.Equipment.Sets.Tier1.Plate
{
    public class PlateArmor : EquipmentItem
    {
        public const string ItemId = "thick_armor";

        public PlateArmor()
        {
            Id = ItemId;
            Name = "Plate Armor";
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
