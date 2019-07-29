namespace Doug.Items.Equipment.Sets.Leather
{
    public class LeatherGloves : EquipmentItem
    {
        public const string ItemId = "leather_gloves";

        public LeatherGloves()
        {
            Id = ItemId;
            Name = "Leather Gloves";
            Description = "Some leather gloves.";
            Rarity = Rarity.Common;
            Icon = ":gloves_1:";
            Slot = EquipmentSlot.Gloves;
            Price = 250;
            LevelRequirement = 10;
            AgilityRequirement = 15;

            Stats.Defense = 4;
            Stats.Hitrate = 16;
        }
    }
}
