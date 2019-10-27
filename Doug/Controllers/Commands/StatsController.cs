using System.Threading.Tasks;
using Doug.Commands;
using Doug.Controllers.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Doug.Controllers.Commands
{
    [Route("cmd/[controller]")]
    [ApiController]
    public class StatsController : ControllerBase
    {
        private readonly IStatsCommands _statsCommands;

        public StatsController(IStatsCommands statsCommands)
        {
            _statsCommands = statsCommands;
        }

        [HttpPost("profile")]
        public async Task<ActionResult> Profile([FromForm]SlackCommandDto slackCommand)
        {
            var result = await _statsCommands.Profile(slackCommand.ToCommand());
            return Ok(result.Message);
        }

        [HttpPost("balance")]
        public ActionResult Balance([FromForm]SlackCommandDto slackCommand)
        {
            var result = _statsCommands.Balance(slackCommand.ToCommand());
            return Ok(result.Message);
        }

        [HttpPost("equipment")]
        public async Task<ActionResult> Equipment([FromForm]SlackCommandDto slackCommand)
        {
            var result = await _statsCommands.Equipment(slackCommand.ToCommand());
            return Ok(result.Message);
        }
    }
}