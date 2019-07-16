using System.Collections.Generic;
using System.Linq;
using Doug.Models;

namespace Doug.Items
{
    public class EquipmentItem : Item
    {
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
        public int Stamina { get; set; }
        public EquipmentSlot Slot { get; set; }
        public int LevelRequirement { get; set; }

        protected EquipmentItem() { }

        public override bool IsEquipable()
        {
            return true;
        }

        public IEnumerable<string> GetDisplayAttributeList()
        {
            var attributes =  new List<string>
            {
                DisplayAttribute(DougMessages.ItemLevel, LevelRequirement),
                DisplayAttribute(DougMessages.ItemAttack, Attack),
                DisplayAttribute(DougMessages.ItemDefense, Defense),
                DisplayAttribute(DougMessages.ItemHitrate, Hitrate),
                DisplayAttribute(DougMessages.ItemDodge, Dodge),
                DisplayAttribute(DougMessages.ItemStrength, Strength),
                DisplayAttribute(DougMessages.ItemAgility, Agility),
                DisplayAttribute(DougMessages.ItemStamina, Stamina),
                DisplayAttribute(DougMessages.ItemConstitution, Constitution),
                DisplayAttribute(DougMessages.ItemLuck, Luck)
            };

            return attributes.Where(attr => !string.IsNullOrEmpty(attr));
        }

        private string DisplayAttribute(string text, int attribute)
        {
            return attribute == 0 ? string.Empty : string.Format(text, attribute);
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
