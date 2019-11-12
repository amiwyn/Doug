using Doug.Monsters.Droptables;

namespace Doug.Monsters
{
    public class AnotherWeeb : Monster
    {
        public const string MonsterId = "keven";

        public AnotherWeeb()
        {
            Id = MonsterId;
            Name = "Another weeb";
            Description = "This one looks like Keven.";
            Image = "http://cdn4.sussexdirectories.com/rms/rms_photos/sized/05/40/184005-256670-3_320x400.jpg?pu=1391451079";
            Level = 46;
            ExperienceValue = 568;
            MaxHealth = Health = 2316;
            MinAttack = 431;
            MaxAttack = 683;
            Hitrate = 73;
            Dodge = 57;
            Defense = 95;
            Resistance = 36;
            AttackCooldown = 20;
            DamageType = Models.Combat.DamageType.Physical;
            DropTable = StRochTable.Drops;
        }
    }
}
