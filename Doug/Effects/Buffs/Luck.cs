namespace Doug.Effects.Buffs
{
    public class Luck : Buff
    {
        public const string EffectId = "luck";

        public Luck()
        {
            Id = EffectId;
            Name = "Luck";
            Description = "You gain +35 luck";
            Rank = Rank.Enchanted;
            Icon = ":clover:";

            Luck = 35;
        }
    }
}
