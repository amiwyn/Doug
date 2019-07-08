using System.Threading.Tasks;
using Doug.Commands;
using Doug.Controllers.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Doug.Controllers
{
    [Route("cmd/[controller]")]
    [ApiController]
    public class CombatController : ControllerBase
    {
        private readonly ICombatCommands _combatCommands;

        public CombatController(ICombatCommands combatCommands)
        {
            _combatCommands = combatCommands;
        }

        [HttpPost("steal")]
        public ActionResult Steal([FromForm]SlackCommandDto slackCommand)
        {

            var result = _combatCommands.Steal(slackCommand.ToCommand());
            return Ok(result.Message);
        }

        [HttpPost("attack")]
        public async Task<ActionResult> Attack([FromForm]SlackCommandDto slackCommand)
        {
            var result = await _combatCommands.Attack(slackCommand.ToCommand());
            return Ok(result.Message);
        }
    }
}