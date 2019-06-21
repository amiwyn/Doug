using System;

namespace Doug.Services
{
    public interface IRandomService
    {
        bool RollAgainstOpponent(double userChances, double opponentChances);
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
    }
}
