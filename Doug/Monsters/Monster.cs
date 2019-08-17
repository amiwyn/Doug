using System;
using System.Collections.Generic;
using Doug.Items;
using Doug.Models;
using Doug.Models.Combat;

namespace Doug.Monsters
{
    public abstract class Monster
    {
        public string Id { get; set; }
        public int Health { get; set; }
        public int Level { get; set; }
        public int ExperienceValue { get; set; }
        public string Image { get; set; }
        public DamageType DamageType { get; set; }
        public Dictionary<LootItem, double> DropTable { get; set; }
        public string Region { get; set; }

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
