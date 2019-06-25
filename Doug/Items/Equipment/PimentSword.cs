using Doug.Models;

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
            Price = 224;

            Attack = 24;
        }

        public override string OnFlaming(Command command, string slur)
        {
            return slur.Replace($"<@{command.GetTargetUserId()}>", $"<@{command.UserId}>");
        }
    }
}
