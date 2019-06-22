using Doug.Items.Consumables;
using Doug.Items.Equipment;
using Doug.Items.Misc;

namespace Doug.Items
{
    public static class ItemFactory
    {
        public const string AwakeningOrb = "awakening_orb";
        public const string LuckyDice = "lucky_dice";
        public const string BurglarBoots = "burglar_boots";
        public const string GreedyGloves = "greedy_gloves";
        public const string NormalEnergyDrink = "normal_energy_drink";
        public const string PimentSword = "piment_sword";
        public const string Apple = "apple";
        public const string DevilsContract = "devil_contract";

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
                case PimentSword:
                    return new PimentSword();
                case Apple:
                    return new Apple();
                case DevilsContract:
                    return new DevilContract();
                default:
                    return new Default();
            }
        }
    }
}
