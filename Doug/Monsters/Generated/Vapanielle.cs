using Doug.Monsters.Droptables;

namespace Doug.Monsters
{
    public class Vapanielle : Monster
    {
        public const string MonsterId = "vapane";

        public Vapanielle()
        {
            Id = MonsterId;
            Name = "Vapanielle";
            Description = "Smokes alot.";
            Image = "https://i.imgur.com/PK51VpL.jpg";
            Level = 14;
            ExperienceValue = 312;
            MaxHealth = Health = 396;
            MinAttack = 86;
            MaxAttack = 145;
            Hitrate = 27;
            Dodge = 22;
            Defense = 18;
            Resistance = 24;
            AttackCooldown = 30;
            DamageType = Models.Combat.DamageType.Physical;
            DropTable = DropTables.Vanier;
        }
    }
}
