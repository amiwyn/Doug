using Doug.Models;

namespace Doug.Controllers.Dto
{
    public class SlackCommandDto
    {
        // ReSharper disable once InconsistentNaming
        public string channel_id { get; set; }
        // ReSharper disable once InconsistentNaming
        public string user_id { get; set; }
        // ReSharper disable once InconsistentNaming
        public string text { get; set; }

        public Command ToCommand()
        {
            return new Command { ChannelId = channel_id, Text = text, UserId = user_id };
        }
    }
}
