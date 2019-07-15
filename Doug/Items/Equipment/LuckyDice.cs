namespace Doug.Items.Equipment
{
    public class LuckyDice : EquipmentItem
    {
        public const string ItemId = "lucky_dice";

        public LuckyDice()
        {
            Id = ItemId;
            Name = "Lucky Dice";
            Description = "A mysterious dice, people say it was carved in the bones of our ancestors, creepy. This dice will increase your luck at gambling.";
            Rarity = Rarity.Unique;
            Icon = ":lucky_dice:";
            Slot = EquipmentSlot.LeftHand;
            Price = 2674;
            LevelRequirement = 5;

            Luck = 10;
        }

        public override double OnGambling(double chance)
        {
            return chance + 0.05;
        }
    }
}
