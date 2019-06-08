using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Doug.Models;
using Doug.Slack;

namespace Doug.Services
{
    public interface IEventService
    {
        void MessageReceived(MessageEvent messageEvent);
    }

    public class EventService : IEventService
    {
        private readonly ICoffeeBreakService _coffeeBreakService;

        public EventService(ICoffeeBreakService coffeeBreakService)
        {
            _coffeeBreakService = coffeeBreakService;
        }

        public void MessageReceived(MessageEvent message)
        {
            if (message.IsValidCoffeeParrot())
            {
                _coffeeBreakService.CountParrot(message.User, message.Channel);
            }
        }
    }
}
