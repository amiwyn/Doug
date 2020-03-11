using System.Threading.Tasks;
using Doug.Controllers.Dto;
using Doug.Services;
using Microsoft.AspNetCore.Mvc;

namespace Doug.Controllers.Commands
{
    [Route("api/[controller]")]
    [ApiController]
    public class LuaController : ControllerBase
    {
        private readonly ILuaService _luaService;

        public LuaController(ILuaService luaService)
        {
            _luaService = luaService;
        }

        [HttpPost("command")]
        public async Task<ActionResult> Command([FromForm]SlackCommandDto slackCommand)
        {
            var result = await _luaService.ExecuteScript(slackCommand.ToCommand());
            return Ok(result.Message);
        }
    }
}