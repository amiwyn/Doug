namespace Doug.Effects.EquipmentEffects
{
    public class Lucky : EquipmentEffect
    {
        public const string EffectId = "lucky";

        public Lucky()
        {
            Id = EffectId;
            Name = "Lucky";
        }

        public override double OnGambling(double chance)
        {
            return chance + 0.05;
        }
    }
}
