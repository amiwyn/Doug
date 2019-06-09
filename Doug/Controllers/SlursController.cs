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

        [HttpPost("addslur")]
        public ActionResult Addslur([FromForm]SlackCommandDto slackCommand)
        {
            try
            {
                var message = _slursCommands.AddSlur(slackCommand.ToCommand());
                return Ok(message);
            }
            catch (Exception ex)
            {
                return Ok(string.Format(DougMessages.DougError, ex.Message));
            }
        }

        [HttpPost("clean")]
        public async Task<ActionResult> CleanSlurs([FromForm]SlackCommandDto slackCommand)
        {
            try
            {
                await _slursCommands.Clean(slackCommand.ToCommand());
                return Ok();
            }
            catch (Exception ex)
            {
                return Ok(string.Format(DougMessages.DougError, ex.Message));
            }
        }

        [HttpPost("viewslurs")]
        public ActionResult ViewSlurs([FromForm]SlackCommandDto slackCommand)
        {
            try
            {
                var result = _slursCommands.Slurs(slackCommand.ToCommand());
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(string.Format(DougMessages.DougError, ex.Message));
            }
        }

        [HttpPost("wholast")]
        public ActionResult Wholast([FromForm]SlackCommandDto slackCommand)
        {
            try
            {
                var result = _slursCommands.WhoLast(slackCommand.ToCommand());
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(string.Format(DougMessages.DougError, ex.Message));
            }
        }
    }
}