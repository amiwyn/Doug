using Doug.Monsters.Droptables;

namespace Doug.Monsters
{
    public class InstaBitch : Monster
    {
        public const string MonsterId = "bitch";

        public InstaBitch()
        {
            Id = MonsterId;
            Name = "Insta-bitch";
            Description = "So much Instagram followers that she denies your attacks.";
            Image = "https://pbs.twimg.com/media/DKBNrJEVAAAwLy4.jpg";
            Level = 16;
            ExperienceValue = 328;
            MaxHealth = Health = 456;
            MinAttack = 97;
            MaxAttack = 162;
            Hitrate = 30;
            Dodge = 24;
            Defense = 21;
            Resistance = 25;
            AttackCooldown = 30;
            DamageType = Models.Combat.DamageType.Physical;
            DropTable = DropTables.Vanier;
        }
    }
}
