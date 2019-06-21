namespace Doug.Items.Equipment
{
    public class GreedyGloves : Item
    {
        public GreedyGloves()
        {
            Name = "Gloves of Greed";
            Description = "These gloves give you an unquenchable thirst for rupees. Increase the amount of rupees you can steal.";
            Rarity = Rarity.Rare;
            Icon = ":gloves:";
        }

        public override int OnStealingAmount(int amount)
        {
            return amount + 5;
        }
    }
}
