namespace Doug.Items.Equipment.Sets.Tier2.Hunter
{
    public class HunterArmor : EquipmentItem
    {
        public const string ItemId = "hunter_armor";

        public HunterArmor()
        {
            Id = ItemId;
            Name = "Hunter Armor";
            Description = "An armor made to protect yourself.";
            Rarity = Rarity.Common;
            Icon = ":hunter_jacket:";
            Slot = EquipmentSlot.Body;
            Price = 1024;
            LevelRequirement = 20;
            AgilityRequirement = 20;

            Stats.Dodge = 10;
            Stats.Defense = 15;
            Stats.Resistance = 10;
            Stats.EnergyRegen = 1;
            Stats.Constitution = 2;
        }
    }
}