using Doug.Monsters.Droptables;

namespace Doug.Monsters
{
    public class AnimeTiddies : Monster
    {
        public const string MonsterId = "tiddies";

        public AnimeTiddies()
        {
            Id = MonsterId;
            Name = "Anime tiddies";
            Description = "HUMONGOUS";
            Image = "https://thebiem.com/wp-content/uploads/2019/01/Urara-Oikawa.jpg.webp";
            Level = 50;
            ExperienceValue = 600;
            MaxHealth = Health = 2700;
            MinAttack = 500;
            MaxAttack = 790;
            Hitrate = 78;
            Dodge = 61;
            Defense = 110;
            Resistance = 37;
            AttackCooldown = 20;
            DamageType = Models.Combat.DamageType.Magical;
            DropTable = StRochTable.Drops;
        }
    }
}
