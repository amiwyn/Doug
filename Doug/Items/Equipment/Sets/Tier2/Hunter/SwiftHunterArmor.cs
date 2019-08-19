namespace Doug.Items.Equipment.Sets.Tier2.Hunter
{
    public class SwiftHunterArmor : EquipmentItem
    {
        public const string ItemId = "swift_hunter_armor";

        public SwiftHunterArmor()
        {
            Id = ItemId;
            Name = "Swift Hunter Armor";
            Description = "An armor made to protect yourself.";
            Rarity = Rarity.Common;
            Icon = ":hunter_jacket:";
            Slot = EquipmentSlot.Body;
            Price = 1442;
            LevelRequirement = 25;
            AgilityRequirement = 20;

            Stats.Dodge = 10;
            Stats.Defense = 18;
            Stats.Resistance = 12;
            Stats.EnergyRegen = 1;
            Stats.Agility = 2;
        }
    }
}