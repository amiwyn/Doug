namespace Doug.Items.Equipment.Sets.Tier2.Fighter
{
    public class FighterArmor : EquipmentItem
    {
        public const string ItemId = "fighter_armor";

        public FighterArmor()
        {
            Id = ItemId;
            Name = "Fighter Armor";
            Description = "An armor made to protect yourself.";
            Rarity = Rarity.Common;
            Icon = ":fighter_armor:";
            Slot = EquipmentSlot.Body;
            Price = 1024;
            LevelRequirement = 20;
            StrengthRequirement = 20;

            Stats.Defense = 22;
            Stats.Resistance = 10;
            Stats.HealthRegen = 2;
        }
    }
}