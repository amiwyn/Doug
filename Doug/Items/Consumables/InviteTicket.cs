using Doug.Models;
using Doug.Repositories;
using Doug.Slack;

namespace Doug.Items.Consumables
{
    public class InviteTicket : ConsumableItem
    {
        private const string CoffeeChannel = "CB6CAU25U";
        private readonly ISlackWebApi _slack;

        public InviteTicket(IInventoryRepository inventoryRepository, ISlackWebApi slack) : base(inventoryRepository)
        {
            _slack = slack;
            Id = ItemFactory.InviteTicket;
            Name = "Invite Ticket";
            Description = "A gift by the gods ~ Use this item to join the coffee channel.";
            Rarity = Rarity.Uncommon;
            Icon = ":join_ticket:";
            Price = 10;
            IsSellable = false;
        }

        public override string Use(int itemPos, User user, string channel)
        {
            _slack.InviteUser(user.Id, CoffeeChannel);

            return base.Use(itemPos, user, channel);
        }

    }
}
