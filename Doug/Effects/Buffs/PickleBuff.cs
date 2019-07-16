namespace Doug.Effects.Buffs
{
    public class PickleBuff : Buff
    {
        public const string EffectId = "pickle_buff";

        public PickleBuff()
        {
            Id = EffectId;
            Name = "Pickle Buff";
            Description = "You gain +100 health and +15 defense";
            Rank = Rank.Common;
            Icon = ":pickle:";

            Health = 100;
            Defense = 15;
        }
    }
}
