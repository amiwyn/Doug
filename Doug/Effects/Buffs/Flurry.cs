namespace Doug.Effects.Buffs
{
    public class Flurry : Buff
    {
        public const string EffectId = "flurry";

        public Flurry()
        {
            Id = EffectId;
            Name = "Flurry";
            Description = "You gain +100 attack speed, but your crit chance and hitrate are halved.";
            Rank = Rank.Enchanted;
            Icon = ":wave:";

            AttackSpeed = 100;
            HitrateFactor = -50;
            CritChanceFactor = -50;
        }
    }
}
