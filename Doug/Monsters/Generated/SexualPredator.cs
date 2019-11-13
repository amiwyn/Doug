using Doug.Monsters.Droptables;

namespace Doug.Monsters
{
    public class SexualPredator : Monster
    {
        public const string MonsterId = "mouine";

        public SexualPredator()
        {
            Id = MonsterId;
            Name = "Sexual predator";
            Description = "Anyone at uni is at risk.";
            Image = "https://media.licdn.com/dms/image/C5603AQFBcuOelwqn9g/profile-displayphoto-shrink_200_200/0?e=1579132800&v=beta&t=weDV7UxVEaKkZILgVNrizHnjLfxJTeOxwXTkVewoQ60";
            Level = 54;
            ExperienceValue = 632;
            MaxHealth = Health = 3116;
            MinAttack = 575;
            MaxAttack = 907;
            Hitrate = 84;
            Dodge = 66;
            Defense = 127;
            Resistance = 38;
            AttackCooldown = 20;
            DamageType = Models.Combat.DamageType.Physical;
            DropTable = DropTables.University;
        }
    }
}
