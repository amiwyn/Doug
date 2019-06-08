using Doug.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Doug.Controllers
{
    public class SlackCommandDto
    {
        public string channel_id { get; set; }
        public string user_id { get; set; }
        public string text { get; set; }

        public Command ToCommand()
        {
            return new Command { ChannelId = channel_id, Text = text, UserId = user_id };
        }
    }
}
