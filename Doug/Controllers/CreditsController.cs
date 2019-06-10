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
    public class CreditsController : ControllerBase
    {
        private readonly ICreditsCommands _creditsCommands;

        public CreditsController(ICreditsCommands creditsCommands)
        {
            _creditsCommands = creditsCommands;
        }

        [HttpPost("give")]
        public ActionResult Give([FromForm]SlackCommandDto slackCommand)
        {
            try
            {
                _creditsCommands.Give(slackCommand.ToCommand());
                return Ok();
            }
            catch (Exception ex)
            {
                return Ok(string.Format(DougMessages.DougError, ex.Message));
            }
        }

        [HttpPost("stats")]
        public ActionResult Stats([FromForm]SlackCommandDto slackCommand)
        {
            try
            {
                _creditsCommands.Stats(slackCommand.ToCommand());
                return Ok();
            }
            catch (Exception ex)
            {
                return Ok(string.Format(DougMessages.DougError, ex.Message));
            }
        }

        [HttpPost("balance")]
        public ActionResult Balance([FromForm]SlackCommandDto slackCommand)
        {
            try
            {
                var result = _creditsCommands.Balance(slackCommand.ToCommand());
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(string.Format(DougMessages.DougError, ex.Message));
            }
        }

        [HttpPost("gamble")]
        public ActionResult Gamble([FromForm]SlackCommandDto slackCommand)
        {
            try
            {
                _creditsCommands.Gamble(slackCommand.ToCommand());
                return Ok();
            }
            catch (Exception ex)
            {
                return Ok(string.Format(DougMessages.DougError, ex.Message));
            }
        }

        [HttpPost("gamblechallenge")]
        public ActionResult GambleChallenge([FromForm]SlackCommandDto slackCommand)
        {
            try
            {
                _creditsCommands.GambleChallenge(slackCommand.ToCommand());
                return Ok();
            }
            catch (Exception ex)
            {
                return Ok(string.Format(DougMessages.DougError, ex.Message));
            }
        }
    }
}