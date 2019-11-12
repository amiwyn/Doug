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
            Level = 62;
            ExperienceValue = 696;
            MaxHealth = Health = 4044;
            MinAttack = 742;
            MaxAttack = 1167;
            Hitrate = 96;
            Dodge = 75;
            Defense = 164;
            Resistance = 40;
            AttackCooldown = 20;
            DamageType = Models.Combat.DamageType.Physical;
            DropTable = StRochTable.Drops;
        }
    }
}
