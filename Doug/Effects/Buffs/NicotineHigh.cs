namespace Doug.Effects.Buffs
{
    public class NicotineHigh : Buff
    {
        public NicotineHigh()
        {
            Id = EffectFactory.TrollBlessing;
            Name = "Nicotine High";
            Description = "You gain +15 charisma for 5 minutes.";
            Rank = Rank.Common;
            Icon = ":smoking:";
            Charisma = 15;
        }
    }
}
