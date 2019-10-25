using Doug.Models;
using Doug.Repositories;
using Doug.Services;
using Doug.Slack;

namespace Doug.Items.Tickets
{
    public class KickTicket : ConsumableItem
    {
        public const string ItemId = "kick_ticket";

        private readonly ISlackWebApi _slack;
        private readonly IUserService _userService;
        private readonly IEventDispatcher _eventDispatcher;

        public KickTicket(IInventoryRepository inventoryRepository, ISlackWebApi slack, IUserService userService, IEventDispatcher eventDispatcher) : base(inventoryRepository)
        {
            _slack = slack;
            _userService = userService;
            _eventDispatcher = eventDispatcher;
            Id = ItemId;
            Name = "Kick Ticket";
            Description = "This item can be used to kick the user of your choice. I would use it on gab if I were you...";
            Rarity = Rarity.Uncommon;
            Icon = ":kick_ticket:";
            Price = 10;
            IsSellable = false;
        }

        public override string Use(int itemPos, User user, string channel)
        {
            return DougMessages.ItemCantBeUsed;
        }

        public override string Target(int itemPos, User user, User target, string channel)
        {
            base.Use(itemPos, user, channel);

            if (!_eventDispatcher.OnKick(target, user, channel))
            {
                return string.Empty;
            }

            _slack.KickUser(target.Id, channel).Wait();

            _slack.BroadcastMessage(string.Format(DougMessages.UsedItemOnTarget, _userService.Mention(user), GetDisplayName(), _userService.Mention(target)), channel);

            return string.Empty;
        }
    }
}
