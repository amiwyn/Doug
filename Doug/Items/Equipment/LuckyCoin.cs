namespace Doug.Items.Equipment
{
    public class LuckyCoin : EquipmentItem
    {
        public const string ItemId = "lucky_coin";

        public LuckyCoin()
        {
            Id = ItemId;
            Name = "Lucky Coin";
            Description = "A mysterious coin, people say it was carved with bones of our ancestors, creepy. This dice will increase your luck at gambling.";
            Rarity = Rarity.Unique;
            Icon = ":coin:";
            Slot = EquipmentSlot.Neck;
            Price = 2674;
            LevelRequirement = 5;

            Luck = 15;
        }

        public override double OnGambling(double chance)
        {
            return chance + 0.05;
        }
    }
}
