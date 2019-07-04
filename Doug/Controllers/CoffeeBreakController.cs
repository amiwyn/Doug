using System;
using System.Threading.Tasks;
using Doug.Commands;
using Doug.Controllers.Dto;
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
            var result = await _coffeeCommands.JoinSomeone(slackCommand.ToCommand());
            return Ok(result.Message);
        }

        [HttpPost("kick")]
        public async Task<ActionResult> Kick([FromForm]SlackCommandDto slackCommand)
        {
            var result = await _coffeeCommands.KickCoffee(slackCommand.ToCommand());
            return Ok(result.Message);
        }

        [HttpPost("resolve")]
        public async Task<ActionResult> Resolve([FromForm]SlackCommandDto slackCommand)
        {
            var result = await _coffeeCommands.Resolve(slackCommand.ToCommand());
            return Ok(result.Message);

        }

        [HttpPost("skip")]
        public async Task<ActionResult> Skip([FromForm]SlackCommandDto slackCommand)
        {
            var result = await _coffeeCommands.Skip(slackCommand.ToCommand());
            return Ok(result.Message);

        }
    }
}