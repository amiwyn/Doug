using Doug.Models;

namespace Doug.Effects.EquipmentEffects
{
    public class Piment : EquipmentEffect
    {
        public const string EffectId = "piment";

        public Piment()
        {
            Id = EffectId;
            Name = "Piment";
        }

        public override string OnFlaming(Command command, string slur)
        {
            return slur.Replace($"<@{command.GetTargetUserId()}>", $"<@{command.UserId}>");
        }
    }
}
