using Doug.Monsters.Droptables;

namespace Doug.Monsters
{
    public class SexyCatherine : Monster
    {
        public const string MonsterId = "cathou";

        public SexyCatherine()
        {
            Id = MonsterId;
            Name = "Sexy Catherine";
            Description = "Scandalously leggy :eyes:";
            Image = "https://i.imgur.com/06xg1l5.jpg";
            Level = 24;
            ExperienceValue = 392;
            MaxHealth = Health = 776;
            MinAttack = 154;
            MaxAttack = 252;
            Hitrate = 42;
            Dodge = 33;
            Defense = 34;
            Resistance = 29;
            AttackCooldown = 30;
            DamageType = Models.Combat.DamageType.Physical;
            DropTable = DropTables.Parliament;
        }
    }
}
