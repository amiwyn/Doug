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
    public class CasinoController : ControllerBase
    {
        private readonly ICasinoCommands _casinoCommands;

        public CasinoController(ICasinoCommands casinoCommands)
        {
            _casinoCommands = casinoCommands;
        }

        [HttpPost("gamble")]
        public ActionResult Gamble([FromForm]SlackCommandDto slackCommand)
        {
            try
            {
                var result = _casinoCommands.Gamble(slackCommand.ToCommand());
                return Ok(result.Message);
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
                var result = _casinoCommands.GambleChallenge(slackCommand.ToCommand());
                return Ok(result.Message);
            }
            catch (Exception ex)
            {
                return Ok(string.Format(DougMessages.DougError, ex.Message));
            }
        }
    }
}