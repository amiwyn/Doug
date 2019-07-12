using System;
using System.Threading.Tasks;
using Doug.Models;
using Doug.Commands;
using Doug.Repositories;

namespace Doug.Services
{
    public interface IEventService
    {
        Task MessageReceived(SlackEventData slackEventData);
        Task ReactionAdded(SlackEventData slackEventData);
    }

    public class EventService : IEventService
    {
        private readonly ICoffeeService _coffeeBreakService;
        private readonly ISlursCommands _slurs;
        private readonly IGovernmentRepository _governmentRepository;
        private readonly IGovernmentService _governmentService;

        private const string GabId = "UB619L16W";

        public EventService(ICoffeeService coffeeBreakService, ISlursCommands slurs, IGovernmentRepository governmentRepository, IGovernmentService governmentService)
        {
            _coffeeBreakService = coffeeBreakService;
            _slurs = slurs;
            _governmentRepository = governmentRepository;
            _governmentService = governmentService;
        }

        public async Task MessageReceived(SlackEventData slack)
        {
            if (slack.IsValidCoffeeParrot())
            {
                _coffeeBreakService.CountParrot(slack.User, slack.Channel, DateTime.UtcNow);
            }
           
            if (slack.ContainsMcdonaldMention())
            {
                Command command = new Command { ChannelId = slack.Channel, UserId = GabId, Text = $"<@{GabId}|gabriel.fillit>" };
                await _slurs.Flame(command);
            }
        }

        public async Task ReactionAdded(SlackEventData slackEventData)
        {
            var timestamp = slackEventData.Item.Ts;
            var government = _governmentRepository.GetGovernment();

            if (timestamp == government.RevolutionTimestamp)
            {
                await _governmentService.CountVotes(timestamp, slackEventData.Item.Channel);
            }
        }
    }
}
