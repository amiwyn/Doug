namespace Doug.Effects.Buffs
{
    public class SmellSweet : Buff
    {
        public const string EffectId = "smell_sweet";
        public SmellSweet()
        {
            Id = EffectId;
            Name = "Smell Sweet";
            Description = "You gain +10 strength.";
            Rank = Rank.Common;
            Icon = ":smoking:";
            Strength = 10;
        }
    }
}
