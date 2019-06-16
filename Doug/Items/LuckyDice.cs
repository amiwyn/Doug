namespace Doug.Items
{
    public class LuckyDice : Item
    {
        public LuckyDice()
        {
            Name = "Lucky Dice";
            Description = "A mysterious dice, people say it was carved in the bones of our ancestors, creepy. This dice will increase your luck at gambling.";
            Rarity = Rarity.Unique;
        }

        public override double OnGambling(double chance)
        {
            return chance + 0.05;
        }
    }
}
