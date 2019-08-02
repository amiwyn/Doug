using System;
using System.Linq;
using System.Threading.Tasks;
using Doug.Controllers.Dto;
using Doug.Menus;
using Doug.Models;
using Doug.Services.MenuServices;
using Microsoft.AspNetCore.Mvc;

namespace Doug.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class InteractionsController : ControllerBase
    {
        private readonly IShopMenuService _shopMenuService;
        private readonly IStatsMenuService _statsMenuService;
        private readonly IInventoryMenuService _inventoryMenuService;
        private readonly IMonsterMenuService _monsterMenuservice;

        public InteractionsController(IShopMenuService shopMenuService, IStatsMenuService statsMenuService, IInventoryMenuService inventoryMenuService, IMonsterMenuService monsterMenuservice)
        {
            _shopMenuService = shopMenuService;
            _statsMenuService = statsMenuService;
            _inventoryMenuService = inventoryMenuService;
            _monsterMenuservice = monsterMenuservice;
        }

        [HttpPost]
        public async Task<ActionResult> Interaction([FromForm]SlackInteractionDto slackInteraction)
        {
            var interaction = slackInteraction.ToInteraction();
            var action = interaction.GetAction();

            switch (action)
            {
                case Actions.Buy:
                    await _shopMenuService.Buy(interaction);
                    break;
                case Actions.Inventory:
                    await InventoryInteractions(interaction);
                    break;
                case Actions.Attribution:
                    await _statsMenuService.AttributeStatPoint(interaction);
                    break;
                case Actions.InventorySwitch:
                    await _inventoryMenuService.ShowInventory(interaction);
                    break;
                case Actions.EquipmentSwitch:
                    await _inventoryMenuService.ShowEquipment(interaction);
                    break;
                case Actions.Equipment:
                    await EquipmentInteractions(interaction);
                    break;
                case Actions.Give:
                    await _inventoryMenuService.Give(interaction);
                    break;
                case Actions.Target:
                    await _inventoryMenuService.Target(interaction);
                    break;
                case Actions.ShopSwitch:
                    await _shopMenuService.ShopSwitch(interaction);
                    break;
                case Actions.Attack:
                    await _monsterMenuservice.Attack(interaction);
                    break;
                case Actions.Skill:
                    await _monsterMenuservice.Skill(interaction);
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
                    await _inventoryMenuService.UnEquip(interaction);
                    break;
                case EquipmentActions.Info:
                    await _inventoryMenuService.Info(interaction);
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
                    await _inventoryMenuService.Use(interaction);
                    break;
                case InventoryActions.Equip:
                    await _inventoryMenuService.Equip(interaction);
                    break;
                case InventoryActions.Sell:
                    await _shopMenuService.Sell(interaction);
                    break;
                case InventoryActions.Give:
                    await _inventoryMenuService.ShowUserSelect(interaction);
                    break;
                case InventoryActions.Target:
                    await _inventoryMenuService.ShowUserSelect(interaction);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}