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

        private const string GAB_ID = "UFHH6CG3Z";

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

            List<string> Words = new List<string>() { "mcd0", "mcdo", "mc d0", "mc do", "mcdonald", "mc donald", "mcd0nalds", "mcd0nald$", "mcd0n4lds", "mcd0n4ld$", "mc d0n4ld$", "mc d0nald$" };
            bool isMcdonald = message.User == GAB_ID && Words.Contains(message.Text.ToLower());

            if (isMcdonald)
            {
                Command command = new Command() { ChannelId = message.Channel, UserId = GAB_ID, Text = "<@UFHH6CG3Z|nathan.vezina>" };
                _slurs.Flame(command);
            }
        }
    }
}
