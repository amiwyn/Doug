namespace Doug.Items.Equipment.Sets
{
    public class BanditBoots : EquipmentItem
    {
        public const string ItemId = "bandit_boots";

        public BanditBoots()
        {
            Id = ItemId;
            Name = "Bandit Boots";
            Description = "These boots are made of leather.";
            Rarity = Rarity.Common;
            Icon = ":boots_1:";
            Slot = EquipmentSlot.Boots;
            Price = 484;
            LevelRequirement = 15;
            AgilityRequirement = 15;

            Stats.Dodge = 14;
            Stats.Defense = 6;
        }
    }
}