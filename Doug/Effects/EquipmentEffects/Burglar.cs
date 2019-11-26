namespace Doug.Effects.EquipmentEffects
{
    public class Burglar : EquipmentEffect
    {
        public const string EffectId = "burglar";

        public Burglar()
        {
            Id = EffectId;
            Name = "Burglar";
        }

        public override double OnStealingChance(double chance)
        {
            return chance + 0.20;
        }
    }
}
