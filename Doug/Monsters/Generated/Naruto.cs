using Doug.Monsters.Droptables;

namespace Doug.Monsters
{
    public class Naruto : Monster
    {
        public const string MonsterId = "naruto";

        public Naruto()
        {
            Id = MonsterId;
            Name = "Naruto";
            Description = "Kagebushino jutsu!";
            Image = "https://pm1.narvii.com/5714/9eecfb3390d7bc6094262323bdfdab5800a927c4_hq.jpg";
            Level = 42;
            ExperienceValue = 536;
            MaxHealth = Health = 1964;
            MinAttack = 368;
            MaxAttack = 584;
            Hitrate = 67;
            Dodge = 53;
            Defense = 81;
            Resistance = 35;
            AttackCooldown = 20;
            DamageType = Models.Combat.DamageType.Physical;
            DropTable = DropTables.Japan;
        }
    }
}
