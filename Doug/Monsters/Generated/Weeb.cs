using Doug.Monsters.Droptables;

namespace Doug.Monsters
{
    public class Weeb : Monster
    {
        public const string MonsterId = "olivier";

        public Weeb()
        {
            Id = MonsterId;
            Name = "Weeb";
            Description = "He looks like Oli.";
            Image = "https://i.imgur.com/Qbv4lVG.jpg";
            Level = 44;
            ExperienceValue = 552;
            MaxHealth = Health = 2136;
            MinAttack = 399;
            MaxAttack = 633;
            Hitrate = 70;
            Dodge = 55;
            Defense = 88;
            Resistance = 35;
            AttackCooldown = 20;
            DamageType = Models.Combat.DamageType.Physical;
            DropTable = DropTables.Japan;
        }
    }
}
