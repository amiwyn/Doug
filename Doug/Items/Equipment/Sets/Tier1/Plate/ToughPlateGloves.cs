namespace Doug.Items.Equipment.Sets.Tier1.Plate
{
    public class ToughPlateGloves : EquipmentItem
    {
        public const string ItemId = "tough_gloves";

        public ToughPlateGloves()
        {
            Id = ItemId;
            Name = "Tough Plate Gloves";
            Description = "Some thicc gloves.";
            Rarity = Rarity.Common;
            Icon = ":gloves_1:";
            Slot = EquipmentSlot.Gloves;
            Price = 484;
            LevelRequirement = 15;
            StrengthRequirement = 15;

            Stats.Hitrate = 16;
            Stats.Defense = 10;
        }
    }
}