using System;

namespace Doug
{
    public static class Utils
    {
        public static string UserMention(string userId)
        {
            return $"<@{userId}>";
        }

        public static bool IsInTimespan(DateTime currentTime, TimeSpan targetTime, int tolerance)
        {
            TimeSpan start = targetTime.Subtract(TimeSpan.FromMinutes(tolerance));
            TimeSpan end = targetTime.Add(TimeSpan.FromMinutes(tolerance));

            return (currentTime.TimeOfDay > start) && (currentTime.TimeOfDay < end);
        }

        public static bool RollAgainstOpponent(double userChances, double opponentChances) //TODO: extract this into a service to inject && mock
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
