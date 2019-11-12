using Doug.Models.User;
using Doug.Repositories;
using Doug.Slack;

namespace Doug.Items.Tickets
{
    public class BeauceTicket : ConsumableItem
    {
        public const string ItemId = "beauce_ticket";

        private const string Channel = "GQGKCDFFG";
        private readonly ISlackWebApi _slack;

        public BeauceTicket(IInventoryRepository inventoryRepository, ISlackWebApi slack) : base(inventoryRepository)
        {
            _slack = slack;
            Id = ItemId;
            Name = "Ferry ticket to Beauce";
            Description = "Give this to the boat driver to get to Beauce.";
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
