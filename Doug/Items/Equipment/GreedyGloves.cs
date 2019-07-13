namespace Doug.Items.Equipment
{
    public class GreedyGloves : EquipmentItem
    {
        public GreedyGloves()
        {
            Id = ItemFactory.GreedyGloves;
            Name = "Gloves of Greed";
            Description = "These gloves give you an unquenchable thirst for rupees. Increase the amount of rupees you can steal.";
            Rarity = Rarity.Rare;
            Icon = ":gloves:";
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
