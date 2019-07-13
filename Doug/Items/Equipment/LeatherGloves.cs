namespace Doug.Items.Equipment
{
    public class LeatherGloves : EquipmentItem
    {
        public LeatherGloves()
        {
            Id = ItemFactory.LeatherGloves;
            Name = "Leather Gloves";
            Description = "Made of leather. Somewhat durable.";
            Rarity = Rarity.Common;
            Icon = ":gloves1:";
            Slot = EquipmentSlot.Gloves;
            Price = 44;
            LevelRequirement = 1;

            Defense = 5;
        }
    }
}
