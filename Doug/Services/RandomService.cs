using System;
using System.Collections.Generic;
using System.Linq;
using Doug.Models;

namespace Doug.Services
{
    public interface IRandomService
    {
        bool RollAgainstOpponent(double userChances, double opponentChances);
        LootItem RandomFromWeightedTable(DropTable table);
        IEnumerable<LootItem> RandomTableDrop(DropTable table, double modifier);
    }

    public class RandomService : IRandomService
    {
        public bool RollAgainstOpponent(double userChances, double opponentChances)
        {
            var total = userChances + opponentChances;
            var rollResult = new Random().NextDouble() * total;

            if (rollResult < 0)
            {
                return false;
            }

            return rollResult < userChances;
        }

        public LootItem RandomFromWeightedTable(DropTable table)
        {
            var roll = new Random().NextDouble();
            var sum = 0.0;

            foreach (var item in table.Items)
            {
                sum += item.Probability;
                if (sum >= roll)
                {
                    return item;
                }
            }

            return table.Items.Last();
        }

        public IEnumerable<LootItem> RandomTableDrop(DropTable table, double modifier)
        {
            var random = new Random();
            return table.Items.Where(elem => random.NextDouble() < elem.Probability + modifier);
        }
    }
}
