using System.ComponentModel.DataAnnotations.Schema;

namespace Doug.Models
{
    public class RegionMonster
    {
        public string ChannelId { get; set; }
        public string MonsterId { get; set; }

        [NotMapped]
        public Channel Channel { get; set; }
    }
}
