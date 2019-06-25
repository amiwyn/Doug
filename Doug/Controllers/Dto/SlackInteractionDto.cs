using System.Collections.Generic;
using System.Linq;
using Doug.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Doug.Controllers.Dto
{
    public class SlackInteractionDto
    {
        // ReSharper disable once InconsistentNaming
        public string payload { get; set; }

        public Interaction ToInteraction()
        {
            var contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            };

            var jsonSettings = new JsonSerializerSettings
            {
                ContractResolver = contractResolver,
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore
            };

            var slackInteraction = JsonConvert.DeserializeObject<SlackInteraction>(payload, jsonSettings);

            return new Interaction
            {
                Action = slackInteraction.Actions.SingleOrDefault()?.ActionId,
                Value = slackInteraction.Actions.SingleOrDefault()?.Value,
                ChannelId = slackInteraction.Channel.Id,
                UserId = slackInteraction.User.Id
            };
        }
    }

    public class SlackInteraction
    {
        public SlackInteractionUser User { get; set; }
        public SlackInteractionChannel Channel { get; set; }
        public List<SlackInteractionAction> Actions { get; set; }
    }

    public class SlackInteractionUser
    {
        public string Id { get; set; }
    }

    public class SlackInteractionChannel
    {
        public string Id { get; set; }
    }

    public class SlackInteractionAction
    {
        public string ActionId { get; set; }
        public string Value { get; set; }
    }
}
