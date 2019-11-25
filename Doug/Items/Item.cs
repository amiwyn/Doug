using Doug.Models.User;

namespace Doug.Items
{
    public delegate string Action(int itemPos, User user, string channel);
    public delegate string TargetAction(int itemPos, User user, User target, string channel);

    public class Item
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Rarity Rarity { get; set; }
        public string Icon { get; set; }
        public int MaxStack { get; set; }
        public int Price { get; set; }
        public bool IsTradable { get; set; }
        public bool IsSellable { get; set; }
        public string ActionId { get; set; }
        public string TargetActionId { get; set; }


        public Item()
        {
            MaxStack = 99;
            IsTradable = true;
            IsSellable = true;
        }

        public virtual string Use(IActionFactory actionFactory, int itemPos, User user, string channel)
        {
            return actionFactory.CreateAction(ActionId)(itemPos, user, channel);
        }

        public virtual string Target(ITargetActionFactory targetActionFactory, int itemPos, User user, User target, string channel)
        {
            return targetActionFactory.CreateTargetAction(TargetActionId)(itemPos, user, target, channel);
        }

        public virtual bool IsEquipable()
        {
            return false;
        }

        public string GetDisplayName()
        {
            return $"{Icon}*{Name}*";
        }
    }
}
