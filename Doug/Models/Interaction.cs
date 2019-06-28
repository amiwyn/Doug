namespace Doug.Models
{
    public class Interaction
    {
        public string UserId { get; set; }
        public string ChannelId { get; set; }
        public string Action { get; set; }
        public string Value { get; set; }
        public string Timestamp { get; set; }
        public string ResponseUrl { get; set; }
    }
}
