using Doug.Models;

namespace Doug.Items.Equipment
{
    public class PimentSword : EquipmentItem
    {
        public const string ItemId = "piment_sword";

        public PimentSword()
        {
            Id = ItemId;
            Name = "Fucking Sword of Piment";
            Description = "Well.. this sword is really spicy. Still useless, you're kinda dumb of owning it.";
            Rarity = Rarity.Uncommon;
            Icon = ":piment_sword:";
            Slot = EquipmentSlot.RightHand;
            Price = 224;
            LevelRequirement = 5;

            Attack = 24;
        }

        public override string OnFlaming(Command command, string slur)
        {
            return slur.Replace($"<@{command.GetTargetUserId()}>", $"<@{command.UserId}>");
        }
    }
}
