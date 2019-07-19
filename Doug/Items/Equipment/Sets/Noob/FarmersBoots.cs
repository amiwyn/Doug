namespace Doug.Items.Equipment.Sets.Noob
{
    public class FarmersBoots : EquipmentItem
    {
        public const string ItemId = "farmers_boots";

        public FarmersBoots()
        {
            Id = ItemId;
            Name = "Farmer's Boots";
            Description = "These boots are made for walking.";
            Rarity = Rarity.Common;
            Icon = ":noob_boots2:";
            Slot = EquipmentSlot.Boots;
            Price = 50;
            LevelRequirement = 5;

            Defense = 4;
            Dodge = 6;
        }
    }
}
