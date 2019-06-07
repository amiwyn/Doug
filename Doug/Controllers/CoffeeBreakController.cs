using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Doug.Commands;
using Doug.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Doug.Controllers
{
    [Route("cmd/[controller]")]
    [ApiController]
    public class CoffeeBreakController : ControllerBase
    {
        private ICoffeeCommands coffeeCommands;

        public CoffeeBreakController(ICoffeeCommands coffeeCommands)
        {
            this.coffeeCommands = coffeeCommands;
        }

        public class SlackCommand
        {
            public string channel_id { get; set; }
            public string user_id { get; set; }
            public string text { get; set; }

            public Command ToCommand()
            {
                return new Command { ChannelId = channel_id, Text = text, UserId = user_id };
            }
        }

        [HttpPost("joincoffee")]
        public ActionResult JoinCoffee([FromForm]SlackCommand slackCommand)
        {
            coffeeCommands.JoinCoffee(slackCommand.ToCommand());
            return Ok();
        }
    }
}