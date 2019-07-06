using Doug.Models;

namespace Doug.Items
{
    public class EquipmentItem : Item
    {
        public int Attack { get; set; }
        public int Luck { get; set; }
        public int Agility { get; set; }
        public int Charisma { get; set; }
        public int Constitution { get; set; }
        public int Stamina { get; set; }
        public EquipmentSlot Slot { get; set; }
        public int LevelRequirement { get; set; }

        protected EquipmentItem() { }

        public override bool IsEquipable()
        {
            return true;
        }

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

        /// <summary>
        /// Event triggered when the user dies. Return true to prevent the user from dying.
        /// </summary>
        /// <returns></returns>
        public virtual bool OnDeath()
        {
            return false;
        }

        /// <summary>
        /// Event triggered when the user dies from the hand of another. Return true to kill the killer instead.
        /// </summary>
        /// <returns></returns>
        public virtual bool OnDeathByUser()
        {
            return false;
        }
    }
}
