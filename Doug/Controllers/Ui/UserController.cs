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
        private readonly IUserRepository _userRepository;
        private readonly ISlackWebApi _slack;
        private readonly IInventoryService _inventoryService;

        public UserController(IUserRepository userRepository, ISlackWebApi slack, IInventoryService inventoryService)
        {
            _userRepository = userRepository;
            _slack = slack;
            _inventoryService = inventoryService;
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

            return Ok(_inventoryService.Equip(user, id));
        }

        [HttpPost("unequip/{slot}")]
        public IActionResult UnEquipItem(int slot)
        {
            var token = Request.Headers["Authorization"].ToString().Substring(7);
            var user = _userRepository.GetUserByToken(token);

            return Ok(_inventoryService.UnEquip(user, (EquipmentSlot)slot));
        }
    }
}