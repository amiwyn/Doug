using System.Threading.Tasks;
using Doug.Controllers.Ui.Dto;
using Doug.Items;
using Doug.Repositories;
using Doug.Services;
using Doug.Slack;
using Microsoft.AspNetCore.Mvc;

namespace Doug.Controllers.Ui
{
    [Route("ui/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private const string General = "CAZMWHXPU";

        private readonly IUserRepository _userRepository;
        private readonly ISlackWebApi _slack;
        private readonly IInventoryService _inventoryService;
        private readonly IShopService _shopService;

        public UserController(IUserRepository userRepository, ISlackWebApi slack, IInventoryService inventoryService, IShopService shopService)
        {
            _userRepository = userRepository;
            _slack = slack;
            _inventoryService = inventoryService;
            _shopService = shopService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            var token = Request.Headers["Authorization"].ToString().Substring(7);

            var user = _userRepository.GetUserByToken(token);
            var identity = await _slack.Identify(token);

            return Ok(new UserInformation(user, identity.Name, identity.image_48));
        }

        [HttpPost("equip/{id}")]
        public IActionResult EquipItem(int id)
        {
            var token = Request.Headers["Authorization"].ToString().Substring(7);
            var user = _userRepository.GetUserByToken(token);

            var message = _inventoryService.Equip(user, id);

            return Ok(new UserInventory(user, message));
        }

        [HttpPost("unequip/{slot}")]
        public IActionResult UnEquipItem(int slot)
        {
            var token = Request.Headers["Authorization"].ToString().Substring(7);
            var user = _userRepository.GetUserByToken(token);

            var message = _inventoryService.UnEquip(user, (EquipmentSlot) slot);

            return Ok(new UserInventory(user, message));
        }


        [HttpPost("use/{id}")]
        public IActionResult UseItem(int id)
        {
            var token = Request.Headers["Authorization"].ToString().Substring(7);
            var user = _userRepository.GetUserByToken(token);

            var message = _inventoryService.Use(user, id, General);

            return Ok(new UserInventory(user, message));
        }


        [HttpPost("sell/{id}")]
        public IActionResult SellItem(int id)
        {
            var token = Request.Headers["Authorization"].ToString().Substring(7);
            var user = _userRepository.GetUserByToken(token);

            var message = _shopService.Sell(user, id).Message;

            return Ok(new UserInventory(user, message));
        }
    }
}