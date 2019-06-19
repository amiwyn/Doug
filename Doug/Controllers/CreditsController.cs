using System;
using Doug.Commands;
using Doug.Controllers.Dto;
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

        [HttpPost("forbes")]
        public ActionResult Forbes([FromForm]SlackCommandDto slackCommand)
        {
            try
            {
                var result = _creditsCommands.Forbes(slackCommand.ToCommand());
                return Ok(result.Message);
            }
            catch (Exception ex)
            {
                return Ok(string.Format(DougMessages.DougError, ex.Message));
            }
        }

        [HttpPost("leaderboard")]
        public ActionResult Leaderboard([FromForm]SlackCommandDto slackCommand)
        {
            try
            {
                var result = _creditsCommands.Leaderboard(slackCommand.ToCommand());
                return Ok(result.Message);
            }
            catch (Exception ex)
            {
                return Ok(string.Format(DougMessages.DougError, ex.Message));
            }
        }
    }
}