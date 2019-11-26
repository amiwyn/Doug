using Doug.Models.User;

namespace Doug.Items
{
    public class Ticket : Consumable
    {
        public string Channel { get; set; }

        public Ticket()
        {
            Rarity = Rarity.Uncommon;
            Icon = ":join_ticket:";
        }

        public override string Use(IActionFactory actionFactory, int itemPos, User user, string channel)
        {
            base.Use(actionFactory, itemPos, user, channel);
            return actionFactory.Transport(user, Channel);
        }
    }
}
