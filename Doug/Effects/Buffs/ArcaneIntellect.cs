namespace Doug.Effects.Buffs
{
    public class ArcaneIntellect : Buff
    {
        public const string EffectId = "arcaneIntellect";

        public ArcaneIntellect()
        {
            Id = EffectId;
            Name = "Arcane Intellect";
            Description = "You regenerate an additional 6 mana per 5 minutes.";
            Rank = Rank.Enchanted;
            Icon = ":blue_book:";

            EnergyRegen = 6;
        }
    }
}
