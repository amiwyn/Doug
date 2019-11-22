using Doug.Models.Combat;
using Doug.Models.User;
using Doug.Repositories;

namespace Doug.Effects.EquipmentEffects
{
    public class Leeching : EquipmentEffect
    {
        private readonly IStatsRepository _statsRepository;
        public const string EffectId = "leeching";

        public Leeching(IStatsRepository statsRepository)
        {
            _statsRepository = statsRepository;
            Id = EffectId;
            Name = "Leeching";
        }

        public override int OnAttacking(User attacker, ICombatable target, int damage)
        {
            attacker.Health += (int)(damage * 0.05);

            _statsRepository.UpdateHealth(attacker.Id, attacker.Health);

            return base.OnAttacking(attacker, target, damage);
        }
    }
}
