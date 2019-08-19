namespace Doug.Items.Equipment.Sets.Tier1.Leather
{
    public class LeatherBoots : EquipmentItem
    {
        public const string ItemId = "leather_boots";

        public LeatherBoots()
        {
            Id = ItemId;
            Name = "Leather Boots";
            Description = "These boots are made of leather.";
            Rarity = Rarity.Common;
            Icon = ":boots_1:";
            Slot = EquipmentSlot.Boots;
            Price = 260;
            LevelRequirement = 10;
            AgilityRequirement = 15;

            Stats.Defense = 4;
            Stats.Dodge = 10;
        }
    }
}
