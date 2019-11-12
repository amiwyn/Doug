using Doug.Models.User;
using Doug.Repositories;
using Doug.Slack;

namespace Doug.Items.Tickets
{
    public class ParliamentTicket : ConsumableItem
    {
        public const string ItemId = "parliament_ticket";

        private const string Channel = "GQ2UR0W1Z";
        private readonly ISlackWebApi _slack;

        public ParliamentTicket(IInventoryRepository inventoryRepository, ISlackWebApi slack) : base(inventoryRepository)
        {
            _slack = slack;
            Id = ItemId;
            Name = "Bus ticket ";
            Description = "Give this to the bus driver to get to the parliament.";
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
