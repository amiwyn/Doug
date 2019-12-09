using System.Threading.Tasks;
using Doug.Controllers.Ui.Dto;
using Doug.Repositories;
using Doug.Slack;
using Microsoft.AspNetCore.Mvc;

namespace Doug.Controllers.Ui
{
    [Route("ui/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ISlackWebApi _slack;

        public UserController(IUserRepository userRepository, ISlackWebApi slack)
        {
            _userRepository = userRepository;
            _slack = slack;
        }

        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            var token = Request.Headers["Authorization"].ToString().Substring(7);

            var user = _userRepository.GetUserByToken(token);
            var identity = await _slack.Identify(token);

            return Ok(new UserInformation(user, identity.Name, identity.image_48));
        }
    }
}