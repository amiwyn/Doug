using Doug.Controllers.Dto;
using Doug.Services;
using Microsoft.AspNetCore.Mvc;

namespace Doug.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class InteractionsController : ControllerBase
    {
        private readonly IShopService _shopService;

        public InteractionsController(IShopService shopService)
        {
            _shopService = shopService;
        }

        [HttpPost]
        public ActionResult Interaction([FromForm]SlackInteractionDto slackInteraction)
        {
            var interaction = slackInteraction.ToInteraction();

            if (interaction.Action == "buy")
            {
                _shopService.Buy(interaction);
            }
            
            return Ok();
        }
    }
}