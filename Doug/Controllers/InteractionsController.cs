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

        public InteractionsController(IShopService shopService, IInventoryCommands inventoryCommands)
        {
            _shopService = shopService;
            _inventoryCommands = inventoryCommands;
        }

        [HttpPost]
        public ActionResult Interaction([FromForm]SlackInteractionDto slackInteraction)
        {
            var interaction = slackInteraction.ToInteraction();

            if (interaction.Action == "buy")
            {
                _shopService.Buy(interaction);
                return Ok();
            }

            if (interaction.Action == "inventory")
            {
                return Ok(InventoryInteractions(interaction));
            }

            return Ok();
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