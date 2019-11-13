using Doug.Monsters.Droptables;

namespace Doug.Monsters
{
    public class Beaubrun : Monster
    {
        public const string MonsterId = "beaubrun";

        public Beaubrun()
        {
            Id = MonsterId;
            Name = "Beaubrun";
            Description = "Do you love networking enough to endure this?";
            Image = "https://i.imgur.com/X55Vtp7.png";
            Level = 52;
            ExperienceValue = 616;
            MaxHealth = Health = 2904;
            MinAttack = 537;
            MaxAttack = 848;
            Hitrate = 81;
            Dodge = 64;
            Defense = 119;
            Resistance = 37;
            AttackCooldown = 20;
            DamageType = Models.Combat.DamageType.Physical;
            DropTable = DropTables.University;
        }
    }
}
