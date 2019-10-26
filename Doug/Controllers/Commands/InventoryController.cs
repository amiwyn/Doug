using System.Threading.Tasks;
using Doug.Commands;
using Doug.Controllers.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Doug.Controllers
{
    [Route("cmd/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryCommands _inventoryCommands;

        public InventoryController(IInventoryCommands inventoryCommands)
        {
            _inventoryCommands = inventoryCommands;
        }

        [HttpPost("use")]
        public ActionResult Use([FromForm]SlackCommandDto slackCommand)
        {
            var result = _inventoryCommands.Use(slackCommand.ToCommand());
            return Ok(result.Message);
        }

        [HttpPost("give")]
        public ActionResult Give([FromForm]SlackCommandDto slackCommand)
        {
            var result = _inventoryCommands.Give(slackCommand.ToCommand());
            return Ok(result.Message);
        }

        [HttpPost("equip")]
        public ActionResult Equip([FromForm]SlackCommandDto slackCommand)
        {
            var result = _inventoryCommands.Equip(slackCommand.ToCommand());
            return Ok(result.Message);
        }

        [HttpPost("inventory")]
        public async Task<ActionResult> Inventory([FromForm]SlackCommandDto slackCommand)
        {
            var result = await _inventoryCommands.Inventory(slackCommand.ToCommand());
            return Ok(result.Message);
        }
    }
}