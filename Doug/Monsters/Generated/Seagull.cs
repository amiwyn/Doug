using Doug.Monsters.Droptables;

namespace Doug.Monsters
{
    public class Seagull : Monster
    {
        public const string MonsterId = "seagull";

        public Seagull()
        {
            Id = MonsterId;
            Name = "Seagull";
            Description = "A fierce animal with a razor sharp beak.";
            Image = "https://upload.wikimedia.org/wikipedia/commons/f/fb/Seagull_in_flight_by_Jiyang_Chen.jpg";
            Level = 2;
            ExperienceValue = 216;
            MaxHealth = Health = 204;
            MinAttack = 51;
            MaxAttack = 92;
            Hitrate = 10;
            Dodge = 9;
            Defense = 11;
            Resistance = 16;
            AttackCooldown = 30;
            DamageType = Models.Combat.DamageType.Physical;
            DropTable = DropTables.StRoch;
        }
    }
}
