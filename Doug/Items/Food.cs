using Doug.Models.User;

namespace Doug.Items
{
    public class Food : Consumable
    {
        public int HealthAmount { get; set; }
        public int EnergyAmount { get; set; }

        public override string Use(IActionFactory actionFactory, int itemPos, User user, string channel)
        {
            base.Use(actionFactory, itemPos, user, channel);

            return actionFactory.Eat(itemPos, user , HealthAmount, EnergyAmount, GetDisplayName());
        }
    }
}
