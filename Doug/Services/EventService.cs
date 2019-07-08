using System;
using Doug.Models;
using Doug.Commands;

namespace Doug.Services
{
    public interface IEventService
    {
        void MessageReceived(MessageEvent messageEvent);
    }

    public class EventService : IEventService
    {
        private readonly ICoffeeService _coffeeBreakService;
        private readonly ISlursCommands _slurs;

        private const string GabId = "UB619L16W";

        public EventService(ICoffeeService coffeeBreakService, ISlursCommands slurs)
        {
            _coffeeBreakService = coffeeBreakService;
            _slurs = slurs;
        }

        public void MessageReceived(MessageEvent message)
        {
            if (message.IsValidCoffeeParrot())
            {
                _coffeeBreakService.CountParrot(message.User, message.Channel, DateTime.UtcNow);
            }
           
            if (message.ContainsMcdonaldMention())
            {
                Command command = new Command() { ChannelId = message.Channel, UserId = GabId, Text = $"<@{GabId}|gabriel.fillit>" };
                _slurs.Flame(command).Wait();
            }
        }
    }
}
