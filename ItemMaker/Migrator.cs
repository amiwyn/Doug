using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ItemMigrator
{
    class Migrator
    {
        static readonly Regex Regex = new Regex("(?:^|,)(\"(?:[^\"]+|\"\")*\"|[^,]*)", RegexOptions.Compiled);

        protected List<string> Split(string input)
        {
            var list = new List<string>();
            foreach (Match match in Regex.Matches(input))
            {
                var current = match.Value;
                if (0 == current.Length)
                {
                    list.Add("");
                }

                list.Add(current.TrimStart(','));
            }

            return list;
        }

        protected string CreateIcon(string iconRaw)
        {
            var icon = iconRaw;
            if (!iconRaw.StartsWith(':'))
            {
                icon = ":" + icon;
            }

            if (!iconRaw.EndsWith(':'))
            {
                icon += ":";
            }

            return icon;
        }
    }
}
