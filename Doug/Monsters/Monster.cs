using System;
using System.Collections.Generic;
using Doug.Items;
using Doug.Models;
using Doug.Models.Combat;

namespace Doug.Monsters
{
    public abstract class Monster : ICombatable
    {
        public string Id { get; set; }
        public int Health { get; set; }
        public int Level { get; set; }
        public int ExperienceValue { get; set; }
        public string Image { get; set; }
        public DamageType DamageType { get; set; }
        public Dictionary<LootItem, double> DropTable { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public int MaxHealth { get; set; }
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

        protected Monster()
        {
            Health = MaxHealth;
        }

        public Attack AttackTarget(ICombatable target, IEventDispatcher eventDispatcher)
        {
            Attack attack = new PhysicalAttack(this, MinAttack, MaxAttack, Hitrate, Luck);

            if (DamageType == DamageType.Magical)
            {
                attack = new MagicAttack(this, Intelligence);
            }

            return target.ReceiveAttack(attack, eventDispatcher);
        }

        public Attack ReceiveAttack(Attack attack, IEventDispatcher eventDispatcher)
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
                attack.Damage = 0;
                return attack;
            }

            var reducedDamage = attack.Damage - (int)Math.Ceiling(attack.Damage * Resistance * 0.01 + Defense);
            reducedDamage = reducedDamage <= 0 ? 1 : reducedDamage;

            attack.Damage = reducedDamage;
            if (attack.Status == AttackStatus.Critical)
            {
                attack.Damage *= 2;
            }

            Health -= attack.Damage;

            return attack;
        }

        private void ApplyMagicalDamage(int damage)
        {
            Health -= damage;
        }

        public bool IsDead()
        {
            return Health <= 0;
        }

        public TimeSpan GetAttackCooldown()
        {
            return TimeSpan.FromSeconds(3000.0 / (100 + Agility / 2));
        }
    }
}
