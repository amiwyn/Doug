namespace Doug.Effects.EquipmentEffects
{
    public class Greedy : EquipmentEffect
    {
        public const string EffectId = "greedy";

        public Greedy()
        {
            Id = EffectId;
            Name = "Greedy";
        }

        public override int OnStealingAmount(int amount)
        {
            return amount + 5;
        }
    }
}
