using Doug.Monsters.Droptables;

namespace Doug.Monsters
{
    public class Beauceron : Monster
    {
        public const string MonsterId = "beauceron";

        public Beauceron()
        {
            Id = MonsterId;
            Name = "Beauceron";
            Description = "Drunk 110% of the time.";
            Image = "https://i.ytimg.com/vi/3-SeM268sWU/maxresdefault.jpg";
            Level = 66;
            ExperienceValue = 728;
            MaxHealth = Health = 4556;
            MinAttack = 835;
            MaxAttack = 1310;
            Hitrate = 101;
            Dodge = 79;
            Defense = 185;
            Resistance = 41;
            AttackCooldown = 20;
            DamageType = Models.Combat.DamageType.Physical;
            DropTable = StRochTable.Drops;
        }
    }
}
