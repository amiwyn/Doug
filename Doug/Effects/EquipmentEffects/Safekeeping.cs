namespace Doug.Effects.EquipmentEffects
{
    public class Safekeeping : EquipmentEffect
    {
        public const string EffectId = "safekeeping";

        public Safekeeping()
        {
            Id = EffectId;
            Name = "Safekeeping";
        }

        public override double OnStealingChance(double chance)
        {
            return -69;
        }

        public override double OnGettingStolenChance(double chance)
        {
            return -69;
        }
    }
}
