namespace Doug.Items.Equipment.Sets.Tier2.Fighter
{
    public class StrongFighterGloves : EquipmentItem
    {
        public const string ItemId = "strong_fighter_gloves";

        public StrongFighterGloves()
        {
            Id = ItemId;
            Name = "Strong Fighter Gloves";
            Description = "Offer some protection and hitrate.";
            Rarity = Rarity.Common;
            Icon = ":gloves_2:";
            Slot = EquipmentSlot.Gloves;
            Price = 1088;
            LevelRequirement = 25;
            StrengthRequirement = 20;

            Stats.Hitrate = 20;
            Stats.Defense = 14;
        }
    }
}