namespace Doug.Items.Equipment.Sets.Thick
{
    public class ThickerGloves : EquipmentItem
    {
        public const string ItemId = "thicker_gloves";

        public ThickerGloves()
        {
            Id = ItemId;
            Name = "Thicker Gloves";
            Description = "Some thicccc gloves.";
            Rarity = Rarity.Common;
            Icon = ":gloves_1:";
            Slot = EquipmentSlot.Gloves;
            Price = 450;
            LevelRequirement = 15;
            StrengthRequirement = 20;

            Stats.Defense = 10;
            Stats.Hitrate = 14;
            Stats.Constitution = 1;
        }
    }
}
