using System;

namespace Doug.Models.Combat
{
    public class PhysicalAttack : Attack
    {
        public int AttackersHitrate { get; set; }
        public double CriticalFactor { get; set; }
        public int Pierce { get; set; }

        public PhysicalAttack(ICombatable attacker, int minDamage, int maxDamage, int attackersHitrate, double criticalHitChance, double criticalFactor, int pierce) : base(RollAttack(minDamage, maxDamage), attacker)
        {
            AttackersHitrate = attackersHitrate;
            CriticalFactor = criticalFactor;
            Pierce = pierce;

            if (new Random().NextDouble() < criticalHitChance)
            {
                Status = AttackStatus.Critical;
            }
        }

        private static int RollAttack(int minDamage, int maxDamage) => new Random().Next(minDamage, maxDamage);

        public PhysicalAttack(ICombatable attacker, int damage, int attackersHitrate) : base(damage, attacker)
        {
            AttackersHitrate = attackersHitrate;
        }
    }
}
