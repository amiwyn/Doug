namespace Doug.Items.Equipment.Sets.Tier1.Leather
{
    public class LeatherArmor : EquipmentItem
    {
        public const string ItemId = "leather_armor";

        public LeatherArmor()
        {
            Id = ItemId;
            Name = "Leather Armor";
            Description = "An armor made of leather.";
            Rarity = Rarity.Common;
            Icon = ":leather_armor:";
            Slot = EquipmentSlot.Body;
            Price = 387;
            LevelRequirement = 10;
            AgilityRequirement = 15;

            Stats.Defense = 10;
            Stats.Resistance = 7;
            Stats.Dodge = 8;
        }
    }
}
