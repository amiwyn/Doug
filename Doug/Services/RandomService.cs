using System;
using System.Collections.Generic;
using System.Linq;

namespace Doug.Services
{
    public interface IRandomService
    {
        bool RollAgainstOpponent(double userChances, double opponentChances);
        T RandomFromWeightedTable<T>(IEnumerable<KeyValuePair<T, double>> table);
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

        public T RandomFromWeightedTable<T>(IEnumerable<KeyValuePair<T, double>> table)
        {
            var roll = new Random().NextDouble();
            var sum = 0.0;

            var weightedTable = table.ToList();
            foreach (var (key, weight) in weightedTable)
            {
                sum += weight;
                if (sum >= roll)
                {
                    return key;
                }
            }

            return weightedTable.Last().Key;
        }
    }
}
