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
                default:
                    return Ok();
            }
        }

        public class SlackEvent
        {
            public string Token { get; set; }
            public string Challenge { get; set; }
            public string TeamId { get; set; }
            public string ApiAppId { get; set; }
            public MessageEvent Event { get; set; }
            public string Type { get; set; }
            public string EventId { get; set; }
            public string EventTime { get; set; }
        }
    }
}