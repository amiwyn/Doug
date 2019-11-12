using Doug.Models.User;
using Doug.Repositories;
using Doug.Slack;

namespace Doug.Items.Tickets
{
    public class ChibougamauTicket : ConsumableItem
    {
        public const string ItemId = "chibougamau_ticket";

        private const string Channel = "GQ2UR7FQT";
        private readonly ISlackWebApi _slack;

        public ChibougamauTicket(IInventoryRepository inventoryRepository, ISlackWebApi slack) : base(inventoryRepository)
        {
            _slack = slack;
            Id = ItemId;
            Name = "Plane ticket to Chibougamau";
            Description = "Because we all know there's no roads to Chibougamau.";
            Rarity = Rarity.Uncommon;
            Icon = ":join_ticket:";
            Price = 869;
        }

        public override string Use(int itemPos, User user, string channel)
        {
            _slack.InviteUser(user.Id, Channel);

            return base.Use(itemPos, user, channel);
        }

    }
}
