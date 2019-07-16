namespace Doug.Effects.Buffs
{
    public class NicotineHigh : Buff
    {
        public const string EffectId = "nicotine_high";
        public NicotineHigh()
        {
            Id = EffectId;
            Name = "Nicotine High";
            Description = "You gain +15 strength.";
            Rank = Rank.Common;
            Icon = ":smoking:";
            Strength = 15;
        }
    }
}
