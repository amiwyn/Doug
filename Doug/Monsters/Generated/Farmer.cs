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
            Level = 70;
            ExperienceValue = 760;
            MaxHealth = Health = 5100;
            MinAttack = 932;
            MaxAttack = 1462;
            Hitrate = 107;
            Dodge = 83;
            Defense = 206;
            Resistance = 42;
            AttackCooldown = 10;
            DamageType = Models.Combat.DamageType.Physical;
            DropTable = StRochTable.Drops;
        }
    }
}
