using Doug.Items.Consumables;
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
        private const string NormalEnergyDrink = "normal_energy_drink";

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
                case NormalEnergyDrink:
                    return new NormalEnergyDrink();
                default:
                    return new Default();
            }
        }
    }
}
