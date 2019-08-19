namespace Doug.Items.Equipment.Sets.Tier2.Mage
{
    public class MageBoots : EquipmentItem
    {
        public const string ItemId = "mage_boots";

        public MageBoots()
        {
            Id = ItemId;
            Name = "Mage Boots";
            Description = "Offer some protection and some dodge.";
            Rarity = Rarity.Common;
            Icon = ":boots_2:";
            Slot = EquipmentSlot.Boots;
            Price = 824;
            LevelRequirement = 20;
            IntelligenceRequirement = 20;

            Stats.Energy = 50;
            Stats.Dodge = 10;
            Stats.Defense = 10;
        }
    }
}