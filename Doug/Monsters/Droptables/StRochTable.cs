using System.Collections.Generic;
using Doug.Items.Consumables;
using Doug.Items.Misc.Drops;
using Doug.Models.Monsters;

namespace Doug.Monsters.Droptables
{
    public class StRochTable
    {
        public static Dictionary<LootItem, double> Drops = new Dictionary<LootItem, double>
        {
            { new LootItem(new Apple(), 1), 0.05 },
            { new LootItem(new GullFeather(), 1), 0.4 },
            { new LootItem(new SharpBeak(), 1), 0.3 },
            { new LootItem(new IronIngot(), 1), 0.1 }
        };
    }
}
