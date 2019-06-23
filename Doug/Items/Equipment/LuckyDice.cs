namespace Doug.Items.Equipment
{
    public class LuckyDice : EquipmentItem
    {
        public LuckyDice()
        {
            Id = ItemFactory.LuckyDice;
            Name = "Lucky Dice";
            Description = "A mysterious dice, people say it was carved in the bones of our ancestors, creepy. This dice will increase your luck at gambling.";
            Rarity = Rarity.Unique;
            Icon = ":game_die:";
            Slot = EquipmentSlot.LeftHand;
        }

        public override double OnGambling(double chance)
        {
            return chance + 0.05;
        }
    }
}
