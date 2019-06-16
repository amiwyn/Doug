using Doug.Models;
using Doug.Slack;
using System.Collections.Generic;

namespace Doug.Items
{
    public abstract class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Rarity Rarity { get; set; }
        public List<UserItem> UserItems { get; set; }

        public Item()
        {
            UserItems = new List<UserItem>();
        }

        public virtual string OnGettingFlamed(Command command, string slur, ISlackWebApi slack)
        {
            return slur;
        }
    }
}
