using System;
using Doug.Models;
using Doug.Slack;
using System.Collections.Generic;
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
        private readonly ISlackWebApi _slack;
        private readonly ISlursCommands _slurs;

        private const string GAB_ID = "UB619L16W";

        public EventService(ICoffeeService coffeeBreakService, ISlackWebApi slack, ISlursCommands slurs)
        {
            _coffeeBreakService = coffeeBreakService;
            _slack = slack;
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
                Command command = new Command() { ChannelId = message.Channel, UserId = GAB_ID, Text = $"<@{GAB_ID}|gabriel.fillit>" };
                _slurs.Flame(command);
            }
        }
    }
}
