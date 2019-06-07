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
    }
}
