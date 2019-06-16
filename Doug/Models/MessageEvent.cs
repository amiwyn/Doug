using Newtonsoft.Json;

namespace Doug.Models
{
    public class MessageEvent
    {
        private const string GroupType = "channel";

        public string ClientMsgId { get; set; }
        public string Type { get; set; }
        public string Text { get; set; }
        public string User { get; set; }
        public string Ts { get; set; }
        public string Channel { get; set; }
        public string EventTs { get; set; }
        public string ChannelType { get; set; }

        public bool IsValidChannel()
        {
            return ChannelType == GroupType;
        }

        public bool IsValidCoffeeParrot()
        {
            return IsValidChannel() && Text.StartsWith(DougMessages.CoffeeParrotEmoji);
        }
    }
}