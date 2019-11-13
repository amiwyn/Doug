using Doug.Monsters.Droptables;

namespace Doug.Monsters
{
    public class Gabby : Monster
    {
        public const string MonsterId = "gabby";

        public Gabby()
        {
            Id = MonsterId;
            Name = "Gabby";
            Description = "Vive Marine Lepen!";
            Image = "https://i.imgur.com/mgglnMj.jpg";
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
            DamageType = Models.Combat.DamageType.Magical;
            DropTable = DropTables.Beauce;
        }
    }
}
