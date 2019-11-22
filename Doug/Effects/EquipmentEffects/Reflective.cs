using System;
using Doug.Models;

namespace Doug.Effects.EquipmentEffects
{
    public class Reflective : EquipmentEffect
    {
        public const string EffectId = "reflective";

        public Reflective()
        {
            Id = EffectId;
            Name = "Reflective";
        }

        public override string OnGettingFlamed(Command command, string slur)
        {
            if (new Random().Next(2) == 0)
            {
                slur = slur.Replace($"<@{command.GetTargetUserId()}>", $"<@{command.UserId}>");
            }

            return slur;
        }
    }
}
