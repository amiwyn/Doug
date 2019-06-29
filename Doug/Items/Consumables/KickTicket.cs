using Doug.Models;
using Doug.Repositories;
using Doug.Slack;

namespace Doug.Items.Consumables
{
    public class KickTicket : ConsumableItem
    {
        private readonly ISlackWebApi _slack;

        public KickTicket(IInventoryRepository inventoryRepository, ISlackWebApi slack) : base(inventoryRepository)
        {
            _slack = slack;
            Id = ItemFactory.KickTicket;
            Name = "Kick Ticket";
            Description = "This item can be used to kick the user of your choice. I would use it on gab if I were you...";
            Rarity = Rarity.Uncommon;
            Icon = ":ticket:";
            Price = 100;
        }

        public override string Use(int itemPos, User user, string channel)
        {
            return DougMessages.ItemCantBeUsed;
        }

        public override string Target(int itemPos, User user, User target, string channel)
        {
            base.Use(itemPos, user, channel);

            _slack.KickUser(target.Id, channel);

            return string.Empty;
        }
    }
}
