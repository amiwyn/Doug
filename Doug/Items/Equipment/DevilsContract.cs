namespace Doug.Items.Equipment
{
    public class DevilsContract : EquipmentItem
    {
        public const string ItemId = "devil_contract";

        public DevilsContract()
        {
            Id = ItemId;
            Name = "Deal with the Devil";
            Description = "He told you no one could steal from you ever again... you should have read the small text. Block stealing. Cannot Steal.";
            Rarity = Rarity.Legendary;
            Icon = ":contract:";
            Slot = EquipmentSlot.LeftHand;
            Price = 4;
            LevelRequirement = 1;
        }

        public override double OnStealingChance(double chance)
        {
            return -69;
        }

        public override double OnGettingStolenChance(double chance)
        {
            return -69;
        }
    }
}
