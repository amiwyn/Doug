using System.Collections.Generic;
using Doug.Items.Consumables;
using Doug.Items.Misc.Drops;
using Doug.Models.Monsters;

namespace Doug.Monsters.Droptables
{
    public class DropTables
    {
        public static Dictionary<LootItem, double> StRoch = new Dictionary<LootItem, double>
        {
            { new LootItem(new Apple(), 1), 0.15 },
            { new LootItem(new GullFeather(), 1), 0.4 },
            { new LootItem(new IronIngot(), 1), 0.1 }
        };

        public static Dictionary<LootItem, double> Vanier = new Dictionary<LootItem, double>
        {
            { new LootItem(new CoffeeCup(), 1), 0.15 },
            { new LootItem(new GullFeather(), 1), 0.4 },
            { new LootItem(new IronIngot(), 1), 0.1 }
        };

        public static Dictionary<LootItem, double> Parliament = new Dictionary<LootItem, double>
        {
            { new LootItem(new Bread(), 1), 0.15 },
            { new LootItem(new GullFeather(), 1), 0.4 },
            { new LootItem(new SilverIngot(), 1), 0.1 }
        };

        public static Dictionary<LootItem, double> Chibougamau = new Dictionary<LootItem, double>
        {
            { new LootItem(new Bread(), 1), 0.15 },
            { new LootItem(new GullFeather(), 1), 0.4 },
            { new LootItem(new SilverIngot(), 1), 0.1 }
        };

        public static Dictionary<LootItem, double> Japan = new Dictionary<LootItem, double>
        {
            { new LootItem(new Bread(), 1), 0.15 },
            { new LootItem(new GullFeather(), 1), 0.4 },
            { new LootItem(new GoldIngot(), 1), 0.1 }
        };

        public static Dictionary<LootItem, double> University = new Dictionary<LootItem, double>
        {
            { new LootItem(new Bread(), 1), 0.15 },
            { new LootItem(new GullFeather(), 1), 0.4 },
            { new LootItem(new GoldIngot(), 1), 0.1 }
        };

        public static Dictionary<LootItem, double> Beauce = new Dictionary<LootItem, double>
        {
            { new LootItem(new Bread(), 1), 0.15 },
            { new LootItem(new GullFeather(), 1), 0.4 },
            { new LootItem(new MithrilIngot(), 1), 0.1 }
        };
    }
}
