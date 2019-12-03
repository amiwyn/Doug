namespace Doug.Effects.Buffs
{
    public class Berserk : Buff
    {
        public const string EffectId = "berserk";

        public Berserk()
        {
            Id = EffectId;
            Name = "Berserk!";
            Description = "Your strength is increased by 20% and your defense is reduced by 30%.";
            Rank = Rank.Enchanted;
            Icon = ":rage:";

            StrengthFactor = 20;
            DefenseFactor = -30;
        }
    }
}
