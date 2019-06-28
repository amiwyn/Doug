using System.Linq;
using System.Threading.Tasks;
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
        private readonly IStatsService _statsService;
        private readonly IInventoryService _inventoryService;

        public InteractionsController(IShopService shopService, IStatsService statsService, IInventoryService inventoryService)
        {
            _shopService = shopService;
            _statsService = statsService;
            _inventoryService = inventoryService;
        }

        [HttpPost]
        public async Task<ActionResult> Interaction([FromForm]SlackInteractionDto slackInteraction)
        {
            var interaction = slackInteraction.ToInteraction();

            switch (interaction.Action)
            {
                case "buy":
                    await _shopService.Buy(interaction);
                    break;
                case "inventory":
                    await InventoryInteractions(interaction);
                    break;
                case "attribution":
                    _statsService.AttributeStatPoint(interaction);
                    break;
            }

            return Ok();
        }

        private async Task InventoryInteractions(Interaction interaction)
        {
            var action = interaction.Value.Split(":").First();

            switch (action)
            {
                case "use":
                    await _inventoryService.Use(interaction);
                    return; 
                case "equip":
                    await _inventoryService.Equip(interaction);
                    return;
                case "sell":
                    await _shopService.Sell(interaction);
                    return;
            }
        }
    }
}