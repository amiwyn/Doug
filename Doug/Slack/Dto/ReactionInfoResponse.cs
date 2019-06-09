using Doug.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Doug.Slack
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
