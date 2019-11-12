using Doug.Models.User;
using Doug.Repositories;
using Doug.Slack;

namespace Doug.Items.Tickets
{
    public class JapanTicket : ConsumableItem
    {
        public const string ItemId = "japan_ticket";

        private const string Channel = "GQDUYD39S";
        private readonly ISlackWebApi _slack;

        public JapanTicket(IInventoryRepository inventoryRepository, ISlackWebApi slack) : base(inventoryRepository)
        {
            _slack = slack;
            Id = ItemId;
            Name = "Weeb-Train ride Ticket";
            Description = "Use this to go to weeb land. Also known as the China's Japan.";
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
