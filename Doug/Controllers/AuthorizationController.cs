using System.Threading.Tasks;
using Doug.Slack;
using Microsoft.AspNetCore.Mvc;

namespace Doug.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly ISlackWebApi _slack;

        public AuthorizationController(ISlackWebApi slack)
        {
            _slack = slack;
        }

        [HttpGet("{authcode}")]
        public async Task<IActionResult> GetAccessToken(string authcode)
        {
            var token = await _slack.Authorize(authcode);

            if (string.IsNullOrWhiteSpace(token))
            {
                return BadRequest();
            }

            return Ok(token);
        }
    }
}