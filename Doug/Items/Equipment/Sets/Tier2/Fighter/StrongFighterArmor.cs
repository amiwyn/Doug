namespace Doug.Items.Equipment.Sets.Tier2.Fighter
{
    public class StrongFighterArmor : EquipmentItem
    {
        public const string ItemId = "strong_fighter_armor";

        public StrongFighterArmor()
        {
            Id = ItemId;
            Name = "Strong Fighter Armor";
            Description = "An armor made to protect yourself.";
            Rarity = Rarity.Common;
            Icon = ":fighter_armor:";
            Slot = EquipmentSlot.Body;
            Price = 1442;
            LevelRequirement = 25;
            StrengthRequirement = 20;

            Stats.Defense = 24;
            Stats.Resistance = 12;
            Stats.HealthRegen = 2;
        }
    }
}