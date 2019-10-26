using Doug.Models.User;
using Doug.Repositories;
using Doug.Slack;

namespace Doug.Items.Tickets
{
    public class StRochTicket : ConsumableItem
    {
        public const string ItemId = "st_roch_ticket";

        private const string Channel = "GMFLGBL6R";
        private readonly ISlackWebApi _slack;

        public StRochTicket(IInventoryRepository inventoryRepository, ISlackWebApi slack) : base(inventoryRepository)
        {
            _slack = slack;
            Id = ItemId;
            Name = "Bus Ticket to Saint-Roch";
            Description = "Give this to the bus driver to get to Saint-Roch.";
            Rarity = Rarity.Uncommon;
            Icon = ":join_ticket:";
            Price = 420;
        }

        public override string Use(int itemPos, User user, string channel)
        {
            _slack.InviteUser(user.Id, Channel);

            return base.Use(itemPos, user, channel);
        }

    }
}
