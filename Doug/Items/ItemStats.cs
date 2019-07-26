using System;
using System.Collections.Generic;
using System.Linq;

namespace Doug.Items
{
    public class ItemStats
    {
        public int Health { get; set; }
        public int Energy { get; set; }
        public int MaxAttack { get; set; }
        public int MinAttack { get; set; }
        public int Defense { get; set; }
        public int Dodge { get; set; }
        public int Hitrate { get; set; }
        public int AttackSpeed { get; set; }
        public int Resistance { get; set; }
        public int Luck { get; set; }
        public int Agility { get; set; }
        public int Strength { get; set; }
        public int Constitution { get; set; }
        public int Intelligence { get; set; }

        public IEnumerable<string> ToStringList()
        {
            var attributes = new List<string>
            {
                MaxAttack == 0 ? string.Empty : string.Format(DougMessages.ItemAttack, MinAttack, MaxAttack),
                DisplayAttribute(DougMessages.ItemDefense, Defense),
                DisplayAttribute(DougMessages.ItemResistance, Resistance),
                Math.Abs(AttackSpeed) <= 1 ? string.Empty : string.Format(DougMessages.AttackSpeed, AttackSpeed),
                DisplayAttribute(DougMessages.ItemHitrate, Hitrate),
                DisplayAttribute(DougMessages.ItemDodge, Dodge),
                DisplayAttribute(DougMessages.ItemHealth, Health),
                DisplayAttribute(DougMessages.ItemEnergy, Energy),
                DisplayAttribute(DougMessages.ItemStrength, Strength),
                DisplayAttribute(DougMessages.ItemAgility, Agility),
                DisplayAttribute(DougMessages.ItemIntelligence, Intelligence),
                DisplayAttribute(DougMessages.ItemConstitution, Constitution),
                DisplayAttribute(DougMessages.ItemLuck, Luck)
            };

            return attributes.Where(attr => !string.IsNullOrEmpty(attr));
        }

        private string DisplayAttribute(string text, int attribute)
        {
            return attribute == 0 ? string.Empty : string.Format(text, attribute);
        }
    }
}
