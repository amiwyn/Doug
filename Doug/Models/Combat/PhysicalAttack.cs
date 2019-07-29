using System;

namespace Doug.Models.Combat
{
    public class PhysicalAttack : Attack
    {
        public int AttackersHitrate { get; set; }

        public PhysicalAttack(ICombatable attacker, int minDamage, int maxDamage, int attackersHitrate, int attackersLuck) : base(RollAttack(minDamage, maxDamage), attacker)
        {
            AttackersHitrate = attackersHitrate;

            if (new Random().NextDouble() < Math.Sqrt(attackersLuck) * 0.04)
            {
                Damage *= 2;
                Status = AttackStatus.Critical;
            }
        }

        private static int RollAttack(int minDamage, int maxDamage) => new Random().Next(minDamage, maxDamage);
    }
}
