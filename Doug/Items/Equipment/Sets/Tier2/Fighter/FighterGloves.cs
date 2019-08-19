namespace Doug.Items.Equipment.Sets.Tier2.Fighter
{
    public class FighterGloves : EquipmentItem
    {
        public const string ItemId = "fighter_gloves";

        public FighterGloves()
        {
            Id = ItemId;
            Name = "Fighter Gloves";
            Description = "Offer some protection and hitrate.";
            Rarity = Rarity.Common;
            Icon = ":gloves_2:";
            Slot = EquipmentSlot.Gloves;
            Price = 882;
            LevelRequirement = 20;
            StrengthRequirement = 20;

            Stats.Hitrate = 18;
            Stats.Defense = 12;
        }
    }
}