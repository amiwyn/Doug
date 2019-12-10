using System.Linq;
using System.Threading.Tasks;
using Doug.Slack;
using Microsoft.AspNetCore.Mvc;

namespace Doug.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly ISlackWebApi _slack;

        public AuthenticationController(ISlackWebApi slack)
        {
            _slack = slack;
        }

        [HttpPost]
        public async Task<IActionResult> Authenticate()
        {
            var code = Request.Form.Keys.ElementAt(0);
            var token = await _slack.Authorize(code);

            if (string.IsNullOrWhiteSpace(token))
            {
                return BadRequest();
            }

            return Ok(new { token });
        }
    }
}