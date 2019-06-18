using Doug.Items.Equipment;
using Doug.Items.Misc;

namespace Doug.Items
{
    public static class ItemFactory
    {
        private const string AwakeningOrb = "awakening_orb";
        private const string LuckyDice = "lucky_dice";
        private const string BurglarBoots = "burglar_boots";
        private const string GreedyGloves = "greedy_gloves";

        public static Item CreateItem(string itemId)
        {
            switch (itemId)
            {
                case AwakeningOrb:
                    return new AwakeningOrb();
                case LuckyDice:
                    return new LuckyDice();
                case BurglarBoots:
                    return new BurglarBoots();
                case GreedyGloves:
                    return new GreedyGloves();
                default:
                    return new Default();
            }
        }
    }
}
