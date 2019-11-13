using Doug.Monsters.Droptables;

namespace Doug.Monsters
{
    public class Farmer : Monster
    {
        public const string MonsterId = "farmer";

        public Farmer()
        {
            Id = MonsterId;
            Name = "Farmer";
            Description = "Rustic person.";
            Image = "https://upload.wikimedia.org/wikipedia/commons/8/80/Farmer%2C_Nicaragua.jpg";
            Level = 62;
            ExperienceValue = 696;
            MaxHealth = Health = 4044;
            MinAttack = 742;
            MaxAttack = 1167;
            Hitrate = 96;
            Dodge = 75;
            Defense = 164;
            Resistance = 40;
            AttackCooldown = 20;
            DamageType = Models.Combat.DamageType.Physical;
            DropTable = DropTables.Beauce;
        }
    }
}
