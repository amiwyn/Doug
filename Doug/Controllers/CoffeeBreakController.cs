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
            _coffeeCommands = coffeeCommands;
        }

        [HttpPost("joincoffee")]
        public ActionResult JoinCoffee([FromForm]SlackCommandDto slackCommand)
        {
            _coffeeCommands.JoinCoffee(slackCommand.ToCommand());
            return Ok();
        }

        [HttpPost("joinsomeone")]
        public async Task<ActionResult> JoinCoffeeOther([FromForm]SlackCommandDto slackCommand)
        {
            try
            {
                await _coffeeCommands.JoinSomeone(slackCommand.ToCommand());
                return Ok();
            }
            catch (Exception ex)
            {
                return Ok(string.Format(DougMessages.DougError, ex.Message));
            }
        }

        [HttpPost("kick")]
        public async Task<ActionResult> Kick([FromForm]SlackCommandDto slackCommand)
        {
            try
            {
                await _coffeeCommands.KickCoffee(slackCommand.ToCommand());
                return Ok();
            }
            catch (Exception ex)
            {
                return Ok(string.Format(DougMessages.DougError, ex.Message));
            }
        }
    }
}