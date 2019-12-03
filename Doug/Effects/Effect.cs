using Doug.Models;
using Doug.Models.Combat;
using Doug.Models.User;

namespace Doug.Effects
{
    public abstract class Effect
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Rank Rank { get; set; }
        public string Icon { get; set; }

        public int Health { get; set; }
        public int Energy { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int Dodge { get; set; }
        public int Hitrate { get; set; }
        public int AttackSpeed { get; set; }
        public int Pierce { get; set; }
        public int CritChance { get; set; }
        public int Luck { get; set; }
        public int Agility { get; set; }
        public int Strength { get; set; }
        public int Constitution { get; set; }
        public int Intelligence { get; set; }
        public int HealthRegen { get; set; }
        public int FlatHealthRegen { get; set; }
        public int EnergyRegen { get; set; }
        public int FlatEnergyRegen { get; set; }

        public int LuckFactor { get; set; }
        public int AgilityFactor { get; set; }
        public int StrengthFactor { get; set; }
        public int ConstitutionFactor { get; set; }
        public int IntelligenceFactor { get; set; }
        public int DefenseFactor { get; set; }
        public int HitrateFactor { get; set; }
        public int CritChanceFactor { get; set; }
        public int CritDamageFactor { get; set; }
        public int PierceFactor { get; set; }

        public virtual bool IsBuff()
        {
            return true;
        }

        /// <summary>
        /// Event raised when the user get kicked. Return false to prevent the holder from being kicked.
        /// </summary>
        /// <returns></returns>
        public virtual bool OnKick(User kicker, string channel)
        {
            return true;
        }

        public virtual string OnGettingFlamed(Command command, string slur)
        {
            return slur;
        }

        public virtual int OnGettingAttacked(ICombatable attacker, User target, int damage)
        {
            return damage;
        }

        public virtual int OnAttacking(User attacker, ICombatable target, int damage)
        {
            return damage;
        }

        public virtual int OnCriticalHit(User attacker, ICombatable target, int damage, string channel)
        {
            return damage;
        }

        public virtual bool OnAttackedInvincibility(ICombatable attacker, ICombatable target)
        {
            return false;
        }
    }
}
