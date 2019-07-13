namespace Doug.Items.Equipment
{
    public class LeatherBoots : EquipmentItem
    {
        public LeatherBoots()
        {
            Id = ItemFactory.LeatherBoots;
            Name = "Leather Boots";
            Description = "These boots are made for walking.";
            Rarity = Rarity.Common;
            Icon = ":boots1:";
            Slot = EquipmentSlot.Boots;
            Price = 48;
            LevelRequirement = 1;

            Defense = 7;
        }
    }
}
