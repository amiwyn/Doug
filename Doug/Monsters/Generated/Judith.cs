using Doug.Monsters.Droptables;

namespace Doug.Monsters
{
    public class Judith : Monster
    {
        public const string MonsterId = "judith";

        public Judith()
        {
            Id = MonsterId;
            Name = "Judith";
            Description = "GroupB0x";
            Image = "https://media.licdn.com/dms/image/C5603AQH6ynUpQs5aEw/profile-displayphoto-shrink_200_200/0?e=1579132800&v=beta&t=YBwsFWpNq8DVRyvkKz6Il5qkIIHz1gX4ZF2P-GU86Xw";
            Level = 36;
            ExperienceValue = 488;
            MaxHealth = Health = 1496;
            MinAttack = 284;
            MaxAttack = 453;
            Hitrate = 59;
            Dodge = 46;
            Defense = 62;
            Resistance = 33;
            AttackCooldown = 20;
            DamageType = Models.Combat.DamageType.Physical;
            DropTable = DropTables.Chibougamau;
        }
    }
}
