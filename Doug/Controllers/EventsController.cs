using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly IEventService _eventService;

        public EventsController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpGet]
        public ActionResult Test()
        {
            return Ok();
        }

        [HttpPost]
        public ActionResult Event(SlackEvent slackEvent)
        {
            if (slackEvent.Type == "url_verification")
            {
                return Ok(new { challenge = slackEvent.Challenge });
            }

            switch (slackEvent.Event.Type)
            {
                case MessageType:
                    _eventService.MessageReceived(slackEvent.Event);
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