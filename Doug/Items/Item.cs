using Doug.Models;
using Doug.Slack;

namespace Doug.Items
{
    public abstract class Item
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Rarity Rarity { get; set; }

        public virtual string OnGettingFlamed(Command command, string slur, ISlackWebApi slack)
        {
            return slur;
        }

        public virtual double OnGambling(double chance)
        {
            return chance;
        }
    }
}
