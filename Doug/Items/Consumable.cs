using Doug.Models.User;

namespace Doug.Items
{
    public class Consumable : Item
    {
        public Consumable()
        {
            MaxStack = 99;
        }

        public override string Use(IActionFactory actionFactory, int itemPos, User user, string channel)
        {
            base.Use(actionFactory,  itemPos, user, channel);
            return actionFactory.Consume(user, itemPos);
        }
    }
}
