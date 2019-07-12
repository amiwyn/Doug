using System.Threading.Tasks;
using Doug.Models;
using Doug.Services;
using Microsoft.AspNetCore.Mvc;

namespace Doug.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private const string MessageType = "message";
        private const string UrlVerification = "url_verification";
        private const string ReactionAdded = "reaction_added";
        private readonly IEventService _eventService;

        public EventsController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpPost]
        public async Task<ActionResult> Event(SlackEvent slackEvent)
        {
            if (slackEvent.Type == UrlVerification)
            {
                return Ok(new { challenge = slackEvent.Challenge });
            }

            switch (slackEvent.Event.Type)
            {
                case MessageType:
                    await _eventService.MessageReceived(slackEvent.Event);
                    return Ok();
                case ReactionAdded:
                    await _eventService.ReactionAdded(slackEvent.Event);
                    return Ok();
                default:
                    return Ok();
            }
        }

        public class SlackEvent
        {
            public string Challenge { get; set; }
            public SlackEventData Event { get; set; }
            public string Type { get; set; }
        }
    }
}