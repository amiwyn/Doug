namespace Doug.Effects
{
    public class UnknownEffect : Buff
    {
        public UnknownEffect()
        {
            Name = "unknown_effect";
            Description = "It does nothing, congratulations.";
            Rank = Rank.Common;
            Icon = ":no_entry:";
        }
    }
}
