using Doug.Effects;
using Doug.Models.Combat;
using Doug.Models.User;
using Doug.Repositories;
using Doug.Services;

namespace Doug.Items.TargetActions
{
    public class AntiRogue : ItemTargetAction
    {
        private readonly IEventDispatcher _eventDispatcher;
        private readonly ICombatService _combatService;

        public AntiRogue(IEventDispatcher eventDispatcher, ICombatService combatService, IInventoryRepository inventoryRepository) : base(inventoryRepository)
        {
            _eventDispatcher = eventDispatcher;
            _combatService = combatService;
        }

        public override string Activate(int itemPos, User user, User target, string channel)
        {
            var damage = target.Level * target.Agility;
            var attack = new MagicAttack(user, damage);
            target.ReceiveAttack(attack, _eventDispatcher);

            _combatService.DealDamage(user, attack, target, channel).Wait();

            return base.Activate(itemPos, user, target, channel);
        }
    }
}
