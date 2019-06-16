using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Doug.Items;
using Doug.Models;
using Doug.Repositories;
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

        private IUserRepository _userRepository;

        public EventsController(IEventService eventService, IUserRepository userRepository)
        {
            _eventService = eventService;
            _userRepository = userRepository;
        }

        [HttpPost]
        public ActionResult Event(SlackEvent slackEvent)
        {
            if (slackEvent.Type == UrlVerification)
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