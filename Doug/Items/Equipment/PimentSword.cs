using Doug.Models;
using Doug.Slack;

namespace Doug.Items.Equipment
{
    public class PimentSword : EquipmentItem
    {
        public PimentSword()
        {
            Id = ItemFactory.PimentSword;
            Name = "Fucking Sword of Piment";
            Description = "Well.. this sword is really spicy. Still useless, you're kinda dumb of owning it.";
            Rarity = Rarity.Uncommon;
            Icon = ":hot_pepper:";
            Slot = EquipmentSlot.RightHand;
        }

        public override string OnFlaming(Command command, string slur, ISlackWebApi slack)
        {
            return slur.Replace($"<@{command.GetTargetUserId()}>", $"<@{command.UserId}>");
        }
    }
}
