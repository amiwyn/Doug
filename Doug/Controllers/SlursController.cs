using System;
using System.Threading.Tasks;
using Doug.Commands;
using Doug.Controllers.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Doug.Controllers
{
    [Route("cmd/[controller]")]
    [ApiController]
    public class SlursController : ControllerBase
    {
        private readonly ISlursCommands _slursCommands;

        public SlursController(ISlursCommands slursCommands)
        {
            _slursCommands = slursCommands;
        }

        [HttpPost("flame")]
        public async Task<ActionResult> JoinCoffeeOther([FromForm]SlackCommandDto slackCommand)
        {
            var result = await _slursCommands.Flame(slackCommand.ToCommand());
            return Ok(result.Message);
        }

        [HttpPost("addslur")]
        public ActionResult Addslur([FromForm]SlackCommandDto slackCommand)
        {
            var result = _slursCommands.AddSlur(slackCommand.ToCommand());
            return Ok(result.Message);
        }

        [HttpPost("clean")]
        public async Task<ActionResult> CleanSlurs([FromForm]SlackCommandDto slackCommand)
        {
            var result = await _slursCommands.Clean(slackCommand.ToCommand());
            return Ok(result.Message);
        }

        [HttpPost("viewslurs")]
        public ActionResult ViewSlurs([FromForm]SlackCommandDto slackCommand)
        {
            var result = _slursCommands.Slurs(slackCommand.ToCommand());
            return Ok(result.Message);
        }

        [HttpPost("wholast")]
        public ActionResult Wholast([FromForm]SlackCommandDto slackCommand)
        {
            var result = _slursCommands.WhoLast(slackCommand.ToCommand());
            return Ok(result.Message);
        }
    }
}