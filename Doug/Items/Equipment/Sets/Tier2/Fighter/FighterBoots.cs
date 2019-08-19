namespace Doug.Items.Equipment.Sets.Tier2.Fighter
{
    public class FighterBoots : EquipmentItem
    {
        public const string ItemId = "fighter_boots";

        public FighterBoots()
        {
            Id = ItemId;
            Name = "Fighter Boots";
            Description = "Offer some protection and some dodge.";
            Rarity = Rarity.Common;
            Icon = ":boots_2:";
            Slot = EquipmentSlot.Boots;
            Price = 824;
            LevelRequirement = 20;
            StrengthRequirement = 20;

            Stats.Dodge = 10;
            Stats.Defense = 10;
        }
    }
}