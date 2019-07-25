namespace Doug.Items.Equipment.Sets.Noob
{
    public class PeasantBoots : EquipmentItem
    {
        public const string ItemId = "peasant_boots";

        public PeasantBoots()
        {
            Id = ItemId;
            Name = "Peasant Boots";
            Description = "These boots are made for walking.";
            Rarity = Rarity.Common;
            Icon = ":noob_boots1:";
            Slot = EquipmentSlot.Boots;
            Price = 30;
            LevelRequirement = 1;

            Stats.Defense = 3;
            Stats.Dodge = 4;
        }
    }
}
