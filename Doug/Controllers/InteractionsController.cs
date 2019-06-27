using System.Linq;
using Doug.Commands;
using Doug.Controllers.Dto;
using Doug.Models;
using Doug.Services;
using Microsoft.AspNetCore.Mvc;

namespace Doug.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class InteractionsController : ControllerBase
    {
        private readonly IShopService _shopService;
        private readonly IInventoryCommands _inventoryCommands;
        private readonly IStatsService _statsService;

        public InteractionsController(IShopService shopService, IInventoryCommands inventoryCommands, IStatsService statsService)
        {
            _shopService = shopService;
            _inventoryCommands = inventoryCommands;
            _statsService = statsService;
        }

        [HttpPost]
        public ActionResult Interaction([FromForm]SlackInteractionDto slackInteraction)
        {
            var interaction = slackInteraction.ToInteraction();

            switch (interaction.Action)
            {
                case "buy":
                    _shopService.Buy(interaction);
                    return Ok();
                case "inventory":
                    return Ok(InventoryInteractions(interaction));
                case "attribution":
                    _statsService.AttributeStatPoint(interaction);
                    return Ok();
                default:
                    return Ok();
            }
        }

        private string InventoryInteractions(Interaction interaction)
        {
            var components = interaction.Value.Split(":");
            var action = components.First();
            var value = components.Last();
            var command = new Command { ChannelId = interaction.ChannelId, Text = value, UserId = interaction.UserId };

            switch (action)
            {
                case "use":
                    return _inventoryCommands.Use(command).Message;
                case "equip":
                    return _inventoryCommands.Equip(command).Message;
                case "sell":
                    _shopService.Sell(interaction);
                    break;
            }
            return string.Empty;
        }
    }
}