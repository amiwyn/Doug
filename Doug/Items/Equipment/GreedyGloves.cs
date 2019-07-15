namespace Doug.Items.Equipment
{
    public class GreedyGloves : EquipmentItem
    {
        public const string ItemId = "greedy_gloves";

        public GreedyGloves()
        {
            Id = ItemId;
            Name = "Gloves of Greed";
            Description = "These gloves give you an unquenchable thirst for rupees. Increase the amount of rupees you can steal.";
            Rarity = Rarity.Rare;
            Icon = ":greedy_gloves:";
            Slot = EquipmentSlot.Gloves;
            Price = 2250;
            LevelRequirement = 20;

            Attack = 5;
            Strength = 5;
        }

        public override int OnStealingAmount(int amount)
        {
            return amount + 5;
        }
    }
}
