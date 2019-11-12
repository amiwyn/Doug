using Doug.Monsters.Droptables;

namespace Doug.Monsters
{
    public class Hobo : Monster
    {
        public const string MonsterId = "hobo";

        public Hobo()
        {
            Id = MonsterId;
            Name = "Hobo";
            Description = "Don't give him any muffins!";
            Image = "https://pbs.twimg.com/profile_images/2297789186/image.jpg";
            Level = 6;
            ExperienceValue = 248;
            MaxHealth = Health = 236;
            MinAttack = 57;
            MaxAttack = 101;
            Hitrate = 16;
            Dodge = 13;
            Defense = 12;
            Resistance = 20;
            AttackCooldown = 30;
            DamageType = Models.Combat.DamageType.Physical;
            DropTable = StRochTable.Drops;
        }
    }
}
