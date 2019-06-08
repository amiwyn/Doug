using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Doug
{
    public static class Utils
    {
        public static string UserMention(string userId)
        {
            return string.Format("<@{0}>", userId);
        }

        public static bool IsInTimespan(DateTime currentTime, TimeSpan targetTime, int tolerance)
        {
            TimeSpan start = targetTime.Subtract(TimeSpan.FromMinutes(tolerance));
            TimeSpan end = targetTime.Add(TimeSpan.FromMinutes(tolerance));

            return (currentTime.TimeOfDay > start) && (currentTime.TimeOfDay < end);
        }
    }
}
