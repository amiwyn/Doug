using Doug.Models;
using Doug.Slack;

namespace Doug.Items.Equipment
{
    public class PimentSword : Item
    {
        public PimentSword()
        {
            Name = "Fucking Sword of Piment";
            Description = "Well.. this sword is really spicy. Still useless, you're kinda dumb of owning it.";
            Rarity = Rarity.Uncommon;
            Icon = ":hot_pepper:";
        }

        public override string OnFlaming(Command command, string slur, ISlackWebApi slack)
        {
            // Todo: Read slur and replace the flamed user with the sender's name.
            return base.OnFlaming(command, slur, slack);
        }
    }
}
