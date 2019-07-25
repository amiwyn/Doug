using System.Linq;

namespace Doug.Models
{
    public class SlackEventData
    {
        private const string GroupType = "channel";
        private const string GabId = "UB619L16W";

        private string[] Words = new string [] { "mcd0", "mcdo", "mc d0", "mc do", "mcdonald", "mc donald", "mcd0nalds", "mcd0nald$", "mc", "do", "donald", "mcd0n4lds", "mcd0n4ld$", "mc d0n4ld$", "mc d0nald$" };

        public string ClientMsgId { get; set; }
        public string Type { get; set; }
        public string Text { get; set; }
        public string User { get; set; }
        public string Ts { get; set; }
        public string Channel { get; set; }
        public string EventTs { get; set; }
        public string ChannelType { get; set; }
        public SlackEventItem Item { get; set; }

        public bool IsValidChannel()
        {
            return ChannelType == GroupType;
        }

        public bool IsValidCoffeeParrot()
        {
            return IsValidChannel() && Text.StartsWith(DougMessages.CoffeeParrotEmoji);
        }

        public bool ContainsMcdonaldMention()
        {
            return User == GabId && Words.Any(Text.ToLower().Contains);
        }
    }

    public class SlackEventItem
    {
        public string Ts { get; set; }
        public string Channel { get; set; }
    }
}
