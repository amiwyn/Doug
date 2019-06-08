using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Doug.Models
{
    public class Command
    {
        public string ChannelId { get; set; }
        public string UserId { get; set; }
        public string Text { get; set; }

        public string GetTargetUserId()
        {
            var parts = GetArgumentAt(0).Split('|');
            return parts[0].Substring(2);
        }

        public bool IsUserArgument()
        {
            var argument = GetArgumentAt(0);
            return argument.StartsWith("<@");
        }

        public string GetArgumentAt(int index)
        {
            if (Text == null)
            {
                throw new Exception(DougMessages.InvalidArgumentCount);
            }

            var args = Text.Split(' ');
            if (args.Length <= index)
            {
                throw new Exception(DougMessages.InvalidArgumentCount);
            }

            return args[index];
        }
    }
}
