namespace Doug.Items.Equipment.Sets
{
    public class BanditGloves : EquipmentItem
    {
        public const string ItemId = "bandit_gloves";

        public BanditGloves()
        {
            Id = ItemId;
            Name = "Bandit Gloves";
            Description = "Some leather gloves.";
            Rarity = Rarity.Common;
            Icon = ":gloves_1:";
            Slot = EquipmentSlot.Gloves;
            Price = 484;
            LevelRequirement = 15;
            AgilityRequirement = 15;

            Stats.Hitrate = 20;
            Stats.Defense = 6;
        }
    }
}