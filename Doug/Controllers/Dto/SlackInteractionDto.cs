using System.Collections.Generic;
using System.Linq;
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

            var value = slackInteraction.Actions.SingleOrDefault()?.Value;

            if (slackInteraction.Actions.SingleOrDefault()?.Type == "overflow")
            {
                value = slackInteraction.Actions.SingleOrDefault()?.SelectedOption.Value;
            }

            if (slackInteraction.Actions.SingleOrDefault()?.Type == "users_select")
            {
                value = slackInteraction.Actions.SingleOrDefault()?.SelectedUser;
            }

            return new Interaction
            {
                Action = slackInteraction.Actions.SingleOrDefault()?.ActionId,
                Value = value,
                BlockId = slackInteraction.Actions.SingleOrDefault()?.BlockId,
                ChannelId = slackInteraction.Channel.Id,
                UserId = slackInteraction.User.Id,
                Timestamp = slackInteraction.Container.MessageTs,
                ResponseUrl = slackInteraction.ResponseUrl
            };
        }
    }

    public class SlackInteraction
    {
        public SlackInteractionUser User { get; set; }
        public SlackInteractionChannel Channel { get; set; }
        public List<SlackInteractionAction> Actions { get; set; }
        public SlackInteractionContainer Container { get; set; }
        public string ResponseUrl { get; set; }
    }

    public class SlackInteractionUser
    {
        public string Id { get; set; }
    }

    public class SlackInteractionChannel
    {
        public string Id { get; set; }
    }

    public class SlackInteractionContainer
    {
        public string MessageTs { get; set; }
    }

    public class SlackInteractionAction
    {
        public string Type { get; set; }
        public string BlockId { get; set; }
        public string ActionId { get; set; }
        public string Value { get; set; }
        public SlackInteractionOption SelectedOption { get; set; }
        public string SelectedUser { get; set; }
    }

    public class SlackInteractionOption
    {
        public string Value { get; set; }
    }
}
