namespace Doug.Items.Equipment.Sets.Tier1.Plate
{
    public class PlateGloves : EquipmentItem
    {
        public const string ItemId = "thick_gloves";

        public PlateGloves()
        {
            Id = ItemId;
            Name = "Plate Gloves";
            Description = "Some thicc gloves.";
            Rarity = Rarity.Common;
            Icon = ":gloves_1:";
            Slot = EquipmentSlot.Gloves;
            Price = 250;
            LevelRequirement = 15;
            StrengthRequirement = 20;

            Stats.Defense = 8;
            Stats.Hitrate = 14;
        }
    }
}
