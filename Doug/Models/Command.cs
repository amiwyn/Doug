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
            var args = Text.Split(' ');
            var parts = args[0].Split('|');
            return parts[0].Substring(2);
        }
    }
}
