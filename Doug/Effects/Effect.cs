using Doug.Models;
using Doug.Models.Combat;

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
        public int Luck { get; set; }
        public int Agility { get; set; }
        public int Strength { get; set; }
        public int Constitution { get; set; }
        public int Intelligence { get; set; }

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

        public virtual bool OnAttackedInvincibility(ICombatable attacker, ICombatable target)
        {
            return false;
        }
    }
}
