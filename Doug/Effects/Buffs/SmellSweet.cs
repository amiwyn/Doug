namespace Doug.Effects.Buffs
{
    public class SmellSweet : Buff
    {
        public const string EffectId = "smell_sweet";
        public SmellSweet()
        {
            Id = EffectId;
            Name = "Smell Sweet";
            Description = "You gain +40 health";
            Rank = Rank.Common;
            Icon = ":smoking:";

            Health = 40;
        }
    }
}
