using System;
using Doug.Items;
using Doug.Models.Combat;

namespace Doug.Monsters
{
    public class Monster : ICombatable
    {
        public int Health { get; set; }
        public int MinAttack { get; set; }
        public int MaxAttack { get; set; }
        public int Hitrate { get; set; }
        public int Dodge { get; set; }
        public int Resistance { get; set; }
        public int Defense { get; set; }

        public int Luck { get; set; }
        public int Agility { get; set; }
        public int Strength { get; set; }
        public int Constitution { get; set; }
        public int Intelligence { get; set; }

        public DamageType DamageType { get; set; }

        public Attack AttackTarget(ICombatable target, IEventDispatcher eventDispatcher)
        {
            Attack attack = new PhysicalAttack(MinAttack, MaxAttack, Hitrate, Luck);

            if (DamageType == DamageType.Magical)
            {
                attack = new MagicAttack(Intelligence);
            }

            return target.ReceiveAttack(attack);
        }

        public Attack ReceiveAttack(Attack attack)
        {
            if (attack is PhysicalAttack physicalAttack)
            {
                return ApplyPhysicalDamage(physicalAttack);
            }

            ApplyMagicalDamage(attack.Damage);
            return attack;
        }

        private PhysicalAttack ApplyPhysicalDamage(PhysicalAttack attack)
        {
            var missChance = (Dodge - attack.AttackersHitrate) * 0.01;
            if (new Random().NextDouble() < missChance)
            {
                attack.Status = AttackStatus.Missed;
                return attack;
            }

            var reducedDamage = attack.Damage - (attack.Damage * (Resistance / 100) + Defense);
            reducedDamage = reducedDamage <= 0 ? 1 : reducedDamage;

            attack.Damage = reducedDamage;
            Health -= reducedDamage;

            return attack;
        }

        private void ApplyMagicalDamage(int damage)
        {
            Health -= damage;
        }
    }
}
