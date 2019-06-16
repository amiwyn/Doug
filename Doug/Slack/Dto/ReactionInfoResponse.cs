using System.Collections.Generic;
using Doug.Models;

namespace Doug.Slack.Dto
{
    public class ReactionInfoResponse
    {
        public MessageReaction Message { get; set; }
    }

    public class MessageReaction
    {
        public List<Reaction> Reactions { get; set; }
    }
}
