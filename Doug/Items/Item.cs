using Doug.Models;

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
        public bool IsTradable { get; set; }

        protected Item()
        {
            MaxStack = 1;
            IsTradable = true;
        }

        public virtual string Use(int itemPos, User user, string channel)
        {
            return DougMessages.ItemCantBeUsed;
        }

        public virtual string Target(int itemPos, User user, User target, string channel)
        {
            return DougMessages.ItemCantBeUsed;
        }

        public virtual bool IsEquipable()
        {
            return false;
        }
    }
}
