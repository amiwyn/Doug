using System.Collections.Generic;

namespace Doug.Slack
{
    public class Reaction
    {
        public string Name { get; set; }
        public int Count { get; set; }
        public List<string> Users { get; set; }
    }
}
