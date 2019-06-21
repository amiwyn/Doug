using Doug.Models;
using Doug.Repositories;
using Doug.Slack;

namespace Doug.Items
{
    public abstract class Item
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Rarity Rarity { get; set; }
        public string Icon { get; set; }
        public int MaxStack { get; set; }
        public int Price { get; set; }
        public int Luck { get; set; }
        public int Agility { get; set; }
        public int Charisma { get; set; }

        protected Item()
        {
            MaxStack = 1;
        }

        public virtual string Use(int itemPos, User user, IUserRepository userRepository, IStatsRepository statsRepository)
        {
            return string.Format(DougMessages.ItemCantBeUsed, itemPos);
        }

        public virtual string OnGettingFlamed(Command command, string slur, ISlackWebApi slack)
        {
            return slur;
        }

        public virtual string OnFlaming(Command command, string slur, ISlackWebApi slack)
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
    }
}
