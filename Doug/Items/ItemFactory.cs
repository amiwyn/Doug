using Doug.Items.Equipement;
using Doug.Items.Equipment;
using Doug.Items.Misc;

namespace Doug.Items
{
    public static class ItemFactory
    {
        public static Item CreateItem(string itemId)
        {
            switch (itemId)
            {
                case "awakening_orb":
                    return new AwakeningOrb();
                case "lucky_dice":
                    return new LuckyDice();
                case "burglar_boots":
                    return new BurglarBoots();
                case "greedy_gloves":
                    return new GreedyGloves();
                case "piment_Sword":
                    return new PimentSword();
                case "energy_drink":
                    return new EnergyDrink();
                default:
                    return new Default();
            }
        }
    }
}
