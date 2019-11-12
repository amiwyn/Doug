using Doug.Models.User;
using Doug.Repositories;
using Doug.Slack;

namespace Doug.Items.Tickets
{
    public class UniversityTicket : ConsumableItem
    {
        public const string ItemId = "university_ticket";

        private const string Channel = "GQEDEP9K7";
        private readonly ISlackWebApi _slack;

        public UniversityTicket(IInventoryRepository inventoryRepository, ISlackWebApi slack) : base(inventoryRepository)
        {
            _slack = slack;
            Id = ItemId;
            Name = "LPU";
            Description = "The 'Laissez-Passer Universitaire'.";
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
