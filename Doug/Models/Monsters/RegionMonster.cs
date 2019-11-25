using System.ComponentModel.DataAnnotations.Schema;

namespace Doug.Models.Monsters
{
    public class RegionMonster
    {
        public string ChannelId { get; set; }
        public string MonsterId { get; set; }
        public Channel Channel { get; set; }
    }
}
