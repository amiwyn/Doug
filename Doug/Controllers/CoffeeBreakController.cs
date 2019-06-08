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
        private ICoffeeCommands _coffeeCommands;

        public CoffeeBreakController(ICoffeeCommands coffeeCommands)
        {
            this._coffeeCommands = coffeeCommands;
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
            _coffeeCommands.JoinCoffee(slackCommand.ToCommand());
            return Ok();
        }

        [HttpPost("joinsomeone")]
        public async Task<ActionResult> JoinCoffeeOther([FromForm]SlackCommand slackCommand)
        {
            await _coffeeCommands.JoinSomeone(slackCommand.ToCommand());
            return Ok();
        }
    }
}