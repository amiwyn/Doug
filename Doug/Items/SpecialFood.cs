using Doug.Models.User;

namespace Doug.Items
{
    public class SpecialFood : Food
    {
        public string EffectId { get; set; }
        public int Duration { get; set; }

        public override string Use(IActionFactory actionFactory, int itemPos, User user, string channel)
        {
            base.Use(actionFactory, itemPos, user, channel);
            return actionFactory.EatEffect(user, EffectId, Duration);
        }
    }
}
