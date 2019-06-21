namespace Doug.Items.Equipment
{
    public class DevilContract : Item
    {
        public DevilContract()
        {
            Name = "Deal with the Devil";
            Description = "He told you no one could steal from you ever again... you should have read the small text. Block stealing. Cannot Steal.";
            Rarity = Rarity.Legendary;
            Icon = ":page_with_curl:";
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
