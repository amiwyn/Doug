namespace Doug.Items.Equipment.Sets.Noob
{
    public class FarmersGloves : EquipmentItem
    {
        public const string ItemId = "farmers_gloves";

        public FarmersGloves()
        {
            Id = ItemId;
            Name = "Farmer's Gloves";
            Description = "Made of leather. Somewhat durable.";
            Rarity = Rarity.Common;
            Icon = ":noob_gloves:";
            Slot = EquipmentSlot.Gloves;
            Price = 55;
            LevelRequirement = 5;

            Defense = 4;
            Hitrate = 7;
        }
    }
}
