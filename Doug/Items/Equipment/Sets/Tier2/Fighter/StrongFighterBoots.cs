namespace Doug.Items.Equipment.Sets.Tier2.Fighter
{
    public class StrongFighterBoots : EquipmentItem
    {
        public const string ItemId = "strong_fighter_boots";

        public StrongFighterBoots()
        {
            Id = ItemId;
            Name = "Strong Fighter Boots";
            Description = "Offer some protection and some dodge.";
            Rarity = Rarity.Common;
            Icon = ":boots_2:";
            Slot = EquipmentSlot.Boots;
            Price = 1115;
            LevelRequirement = 25;
            StrengthRequirement = 20;

            Stats.Dodge = 12;
            Stats.Defense = 12;
        }
    }
}