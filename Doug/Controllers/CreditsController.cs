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
                var result = _creditsCommands.Give(slackCommand.ToCommand());
                return Ok(result.Message);
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
                var result = _creditsCommands.Stats(slackCommand.ToCommand());
                return Ok(result.Message);
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
                return Ok(result.Message);
            }
            catch (Exception ex)
            {
                return Ok(string.Format(DougMessages.DougError, ex.Message));
            }
        }
    }
}