using Doug.Models.Combat;
using Doug.Models.User;
using Doug.Repositories;

namespace Doug.Effects.Buffs
{
    public class MortuaryGrace : Buff
    {
        private readonly IEffectRepository _effectRepository;
        public const string EffectId = "mortuary_grace";

        public MortuaryGrace(IEffectRepository effectRepository)
        {
            _effectRepository = effectRepository;
            Id = EffectId;
            Name = "Mortuary Grace";
            Description = "You died recently. You cannot be attacked. Attacking will break this effect.";
            Rank = Rank.Common;
            Icon = ":skull:";
        }

        public override bool OnAttackedInvincibility(ICombatable attacker, ICombatable target)
        {
            return true;
        }

        public override int OnAttacking(User attacker, ICombatable target, int damage)
        {
            _effectRepository.RemoveEffect(attacker, EffectId);
            return base.OnAttacking(attacker, target, damage);
        }
    }
}
