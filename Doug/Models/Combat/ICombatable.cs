using Doug.Items;

namespace Doug.Models.Combat
{
    public interface ICombatable
    {
        Attack AttackTarget(ICombatable target, IEventDispatcher eventDispatcher);
        Attack ReceiveAttack(Attack attack, IEventDispatcher eventDispatcher);
    }
}
