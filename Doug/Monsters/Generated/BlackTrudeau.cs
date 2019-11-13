using Doug.Monsters.Droptables;

namespace Doug.Monsters
{
    public class BlackTrudeau : Monster
    {
        public const string MonsterId = "trudeau";

        public BlackTrudeau()
        {
            Id = MonsterId;
            Name = "Black Trudeau";
            Description = "A young man with an interesting sense of humor";
            Image = "https://www.thesun.co.uk/wp-content/uploads/2019/09/trudeau2.jpg";
            Level = 22;
            ExperienceValue = 376;
            MaxHealth = Health = 684;
            MinAttack = 138;
            MaxAttack = 226;
            Hitrate = 39;
            Dodge = 31;
            Defense = 30;
            Resistance = 28;
            AttackCooldown = 30;
            DamageType = Models.Combat.DamageType.Physical;
            DropTable = DropTables.Parliament;
        }
    }
}
