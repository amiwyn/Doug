using System;
using Doug.Models;

namespace Doug.Services
{
    public interface IEventService
    {
        void MessageReceived(MessageEvent messageEvent);
    }

    public class EventService : IEventService
    {
        private readonly ICoffeeService _coffeeBreakService;

        public EventService(ICoffeeService coffeeBreakService)
        {
            _coffeeBreakService = coffeeBreakService;
        }

        public void MessageReceived(MessageEvent message)
        {
            if (message.IsValidCoffeeParrot())
            {
                _coffeeBreakService.CountParrot(message.User, message.Channel, DateTime.UtcNow);
            }
        }
    }
}
