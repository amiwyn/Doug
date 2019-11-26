using System.Collections.Generic;
using Doug.Models.Monsters;

namespace Doug.Models
{
    public enum ChannelType
    {
        Default,
        Pvp,
        Casino,
        Coffee,
        Marketplace,
        Common,
        Region
    }

    public class Channel
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public List<RegionMonster> Monsters { get; set; }
    }
}
