using System;
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
            try
            {
                var result = _inventoryCommands.Use(slackCommand.ToCommand());
                return Ok(result.Message);
            }
            catch (Exception ex)
            {
                return Ok(string.Format(DougMessages.DougError, ex.Message));
            }
        }
    }
}