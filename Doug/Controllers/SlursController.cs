using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Doug.Commands;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Doug.Controllers
{
    [Route("cmd/[controller]")]
    [ApiController]
    public class SlursController : ControllerBase
    {
        private ISlursCommands _slursCommands;

        public SlursController(ISlursCommands slursCommands)
        {
            _slursCommands = slursCommands;
        }

        [HttpPost("flame")]
        public async Task<ActionResult> JoinCoffeeOther([FromForm]SlackCommandDto slackCommand)
        {
            try
            {
                await _slursCommands.Flame(slackCommand.ToCommand());
                return Ok();
            }
            catch (Exception ex)
            {
                return Ok(string.Format(DougMessages.DougError, ex.Message));
            }
        }
    }
}