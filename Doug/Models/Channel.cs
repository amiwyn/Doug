using System.Collections.Generic;

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
        public List<RegionMonster> Monsters { get; set; }
    }
}
