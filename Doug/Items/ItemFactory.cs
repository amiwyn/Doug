using System;
using Doug.Items.Equipment;

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
                default:
                    throw new ArgumentException("Invalid item id");
            }
        }
    }
}
