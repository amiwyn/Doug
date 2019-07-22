using System.Collections.Generic;
using System.Linq;
using Doug.Models;

namespace Doug.Items
{
    public class EquipmentItem : Item
    {
        public ItemStats Stats { get; set; }
        public EquipmentSlot Slot { get; set; }
        public int LevelRequirement { get; set; }

        protected EquipmentItem()
        {
            Stats = new ItemStats();
        }

        public override bool IsEquipable()
        {
            return true;
        }

        public virtual IEnumerable<string> GetDisplayAttributeList()
        {
            return Stats.ToStringList().Prepend(string.Format(DougMessages.ItemLevel, LevelRequirement));
        }

        public bool IsHandSlot()
        {
            return Slot == EquipmentSlot.LeftHand || Slot == EquipmentSlot.RightHand;
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
        /// Event triggered when the user dies. Return false to prevent the user from dying.
        /// </summary>
        /// <returns></returns>
        public virtual bool OnDeath()
        {
            return true;
        }

        /// <summary>
        /// Event triggered when the user dies from the hand of a killer.
        /// </summary>
        /// <returns></returns>
        public virtual void OnDeathByUser(User killer)
        {
        }
    }
}
