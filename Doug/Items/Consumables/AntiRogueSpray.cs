using Doug.Models;
using Doug.Models.Combat;
using Doug.Repositories;
using Doug.Services;
using Doug.Slack;

namespace Doug.Items.Consumables
{
    public class AntiRogueSpray : ConsumableItem
    {
        public const string ItemId = "anti_rogue_spray";

        private readonly IEventDispatcher _eventDispatcher;
        private readonly ISlackWebApi _slack;
        private readonly IUserService _userService;
        private readonly ICombatService _combatService;

        public AntiRogueSpray(IInventoryRepository inventoryRepository, IEventDispatcher eventDispatcher, ISlackWebApi slack, IUserService userService, ICombatService combatService) : base(inventoryRepository)
        {
            _eventDispatcher = eventDispatcher;
            _slack = slack;
            _userService = userService;
            _combatService = combatService;
            Id = ItemId;
            Name = "Invisible anti-rogue spray can";
            Description = "Sold by a mysterious mustachey peddler";
            Rarity = Rarity.Uncommon;
            Icon = ":invisible:";
            Price = 100;
        }

        public override string Use(int itemPos, User user, string channel)
        {
            return DougMessages.ItemCantBeUsed;
        }

        public override string Target(int itemPos, User user, User target, string channel)
        {
            base.Use(itemPos, user, channel);

            var message = string.Format(DougMessages.UsedItemOnTarget, _userService.Mention(user), Name, _userService.Mention(target));
            _slack.BroadcastMessage(message, channel);

            var damage = target.Level * target.Agility;
            var attack = new MagicAttack(user, damage);
            target.ReceiveAttack(attack, _eventDispatcher);

            _combatService.DealDamage(user, attack, target, channel);

            return string.Empty;
        }
    }
}
