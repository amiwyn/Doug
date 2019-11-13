using Doug.Monsters.Droptables;

namespace Doug.Monsters
{
    public class Rat : Monster
    {
        public const string MonsterId = "rat";

        public Rat()
        {
            Id = MonsterId;
            Name = "Rat";
            Description = "A rat.";
            Image = "https://pbs.twimg.com/profile_images/1133945459242917888/lCkImptU_400x400.jpg";
            Level = 8;
            ExperienceValue = 264;
            MaxHealth = Health = 264;
            MinAttack = 62;
            MaxAttack = 108;
            Hitrate = 19;
            Dodge = 15;
            Defense = 13;
            Resistance = 21;
            AttackCooldown = 30;
            DamageType = Models.Combat.DamageType.Physical;
            DropTable = DropTables.StRoch;
        }
    }
}
