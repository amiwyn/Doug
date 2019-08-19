namespace Doug.Items.Equipment.Sets.Tier2.Hunter
{
    public class HunterBeret : EquipmentItem
    {
        public const string ItemId = "hunter_hat";

        public HunterBeret()
        {
            Id = ItemId;
            Name = "Hunter Beret";
            Description = "Vive la France!";
            Rarity = Rarity.Common;
            Icon = ":hunter_beret:";
            Slot = EquipmentSlot.Head;
            Price = 712;
            LevelRequirement = 20;
            AgilityRequirement = 20;

            Stats.Dodge = 8;
            Stats.Defense = 4;
            Stats.Constitution = 1;
        }
    }
}