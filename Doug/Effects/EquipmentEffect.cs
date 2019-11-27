using Doug.Models;
using Doug.Models.Combat;
using Doug.Models.User;

namespace Doug.Effects
{
    public class EquipmentEffect
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public virtual string OnGettingFlamed(Command command, string slur)
        {
            return slur;
        }

        public virtual string OnFlaming(Command command, string slur)
        {
            return slur;
        }

        public virtual double OnGambling(double chance)
        {
            return chance;
        }

        public virtual double OnGettingStolenChance(double chance)
        {
            return chance;
        }

        public virtual double OnStealingChance(double chance)
        {
            return chance;
        }

        public virtual int OnStealingAmount(int amount)
        {
            return amount;
        }

        public virtual string OnMention(string mention)
        {
            return mention;
        }

        public virtual bool OnDeath()
        {
            return true;
        }

        public virtual void OnDeathByUser(User killer)
        {
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
    }
}
