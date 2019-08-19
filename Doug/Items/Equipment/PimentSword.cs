using Doug.Items.WeaponType;
using Doug.Models;

namespace Doug.Items.Equipment
{
    public class PimentSword : Sword
    {
        public const string ItemId = "piment_sword";

        public PimentSword()
        {
            Id = ItemId;
            Name = "Fucking Sword of Piment";
            Description = "Well.. this sword is really spicy. Still useless, you're kinda dumb of owning it.";
            Rarity = Rarity.Uncommon;
            Icon = ":piment_sword:";         
            Price = 224;
            LevelRequirement = 5;
            Stats.MinAttack = 18;
            Stats.MaxAttack = 24;
        }

        public override string OnFlaming(Command command, string slur)
        {
            return slur.Replace($"<@{command.GetTargetUserId()}>", $"<@{command.UserId}>");
        }
    }
}
