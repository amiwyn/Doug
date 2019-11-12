using Doug.Monsters.Droptables;

namespace Doug.Monsters
{
    public class APepe : Monster
    {
        public const string MonsterId = "meme";

        public APepe()
        {
            Id = MonsterId;
            Name = "A pepe";
            Description = "Blaze it";
            Image = "https://www.fccnn.com/news/article796212.ece/alternates/BASE_LANDSCAPE/pepe.jpg";
            Level = 4;
            ExperienceValue = 232;
            MaxHealth = Health = 216;
            MinAttack = 53;
            MaxAttack = 95;
            Hitrate = 13;
            Dodge = 11;
            Defense = 11;
            Resistance = 18;
            AttackCooldown = 30;
            DamageType = Models.Combat.DamageType.Physical;
            DropTable = StRochTable.Drops;
        }
    }
}
