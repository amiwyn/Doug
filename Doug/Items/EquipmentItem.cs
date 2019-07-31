using System.Collections.Generic;
using System.Linq;
using Doug.Models;
using Doug.Models.Combat;

namespace Doug.Items
{
    public abstract class EquipmentItem : Item
    {
        public int LuckRequirement { get; set; }
        public int AgilityRequirement { get; set; }
        public int StrengthRequirement { get; set; }
        public int IntelligenceRequirement { get; set; }
        public int ConstitutionRequirement { get; set; }

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
            var attributes = new List<string>
            {
                string.Format(DougMessages.ItemLevel, LevelRequirement),
                DisplayRequirement(DougMessages.Luck, LuckRequirement),
                DisplayRequirement(DougMessages.Agi, AgilityRequirement),
                DisplayRequirement(DougMessages.Str, StrengthRequirement),
                DisplayRequirement(DougMessages.Int, IntelligenceRequirement),
                DisplayRequirement(DougMessages.Con, ConstitutionRequirement),
            };
            var requirements = attributes.Where(attr => !string.IsNullOrEmpty(attr)).ToList();
            return requirements.Concat(Stats.ToStringList());
        }

        private string DisplayRequirement(string text, int requirement)
        {
            return requirement == 0 ? string.Empty : $"*{requirement} {text}*";
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

        public virtual int OnGettingAttacked(ICombatable attacker, User target, int damage)
        {
            return damage;
        }

        public virtual int OnAttacking(User attacker, ICombatable target, int damage)
        {
            return damage;
        }
    }
}
