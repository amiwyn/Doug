namespace Doug.Items.Equipment.Sets
{
    public class BanditArmor : EquipmentItem
    {
        public const string ItemId = "bandit_armor";

        public BanditArmor()
        {
            Id = ItemId;
            Name = "Bandit Armor";
            Description = "An armor made of leather.";
            Rarity = Rarity.Common;
            Icon = ":leather_armor:";
            Slot = EquipmentSlot.Body;
            Price = 669;
            LevelRequirement = 15;
            AgilityRequirement = 15;

            Stats.Dodge = 10;
            Stats.Defense = 12;
            Stats.Resistance = 8;
        }
    }
}