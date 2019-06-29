using System;
using System.Linq;
using System.Threading.Tasks;
using Doug.Controllers.Dto;
using Doug.Menus;
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
            var action = interaction.GetAction();

            switch (action)
            {
                case Actions.Buy:
                    await _shopService.Buy(interaction);
                    break;
                case Actions.Inventory:
                    await InventoryInteractions(interaction);
                    break;
                case Actions.Attribution:
                    await _statsService.AttributeStatPoint(interaction);
                    break;
                case Actions.InventorySwitch:
                    await _inventoryService.ShowInventory(interaction);
                    break;
                case Actions.EquipmentSwitch:
                    await _inventoryService.ShowEquipment(interaction);
                    break;
                case Actions.Equipment:
                    await EquipmentInteractions(interaction);
                    break;
                case Actions.Give:
                    await _inventoryService.Give(interaction);
                    break;
                case Actions.Target:
                    await _inventoryService.Target(interaction);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return Ok();
        }

        private async Task EquipmentInteractions(Interaction interaction)
        {
            Enum.TryParse(interaction.Value.Split(":").First(), out EquipmentActions action);

            switch (action)
            {
                case EquipmentActions.UnEquip:
                    await _inventoryService.UnEquip(interaction);
                    break;
                case EquipmentActions.Info:
                    await _inventoryService.Info(interaction);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private async Task InventoryInteractions(Interaction interaction)
        {
            Enum.TryParse(interaction.Value.Split(":").First(), out InventoryActions action);

            switch (action)
            {
                case InventoryActions.Use:
                    await _inventoryService.Use(interaction);
                    break;
                case InventoryActions.Equip:
                    await _inventoryService.Equip(interaction);
                    break;
                case InventoryActions.Sell:
                    await _shopService.Sell(interaction);
                    break;
                case InventoryActions.Give:
                    await _inventoryService.ShowUserSelect(interaction);
                    break;
                case InventoryActions.Target:
                    await _inventoryService.ShowUserSelect(interaction);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}