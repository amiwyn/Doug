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

        protected Item()
        {
            MaxStack = 1;
        }

        public virtual string Use(int itemPos, User user)
        {
            return string.Format(DougMessages.ItemCantBeUsed, itemPos);
        }

        public virtual bool IsEquipable()
        {
            return false;
        }
    }
}
