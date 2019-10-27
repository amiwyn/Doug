using Doug.Models.User;
using Doug.Repositories;
using Doug.Slack;

namespace Doug.Items.Tickets
{
    public class VanierTicket : ConsumableItem
    {
        public const string ItemId = "vanier_ticket";

        private const string Channel = "GMHRTPQ94";
        private readonly ISlackWebApi _slack;

        public VanierTicket(IInventoryRepository inventoryRepository, ISlackWebApi slack) : base(inventoryRepository)
        {
            _slack = slack;
            Id = ItemId;
            Name = "Bus Ticket to Vanier";
            Description = "Give this to the bus driver to get to Vanier.";
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
