namespace Doug.Items.Equipment.Sets.Thick
{
    public class ThickGloves : EquipmentItem
    {
        public const string ItemId = "thick_gloves";

        public ThickGloves()
        {
            Id = ItemId;
            Name = "Thick Gloves";
            Description = "Some thicc gloves.";
            Rarity = Rarity.Common;
            Icon = ":gloves_1:";
            Slot = EquipmentSlot.Gloves;
            Price = 250;
            LevelRequirement = 10;
            StrengthRequirement = 15;

            Stats.Defense = 8;
            Stats.Hitrate = 10;
        }
    }
}
