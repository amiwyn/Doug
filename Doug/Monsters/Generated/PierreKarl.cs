using Doug.Monsters.Droptables;

namespace Doug.Monsters
{
    public class PierreKarl : Monster
    {
        public const string MonsterId = "pkp";

        public PierreKarl()
        {
            Id = MonsterId;
            Name = "Pierre-Karl";
            Description = "He's still in the game.. in his heart";
            Image = "https://media2.ledevoir.com/images_galerie/nwd_143428_109684/pierre-karl-peladeau.jpg";
            Level = 28;
            ExperienceValue = 424;
            MaxHealth = Health = 984;
            MinAttack = 192;
            MaxAttack = 310;
            Hitrate = 47;
            Dodge = 37;
            Defense = 42;
            Resistance = 30;
            AttackCooldown = 30;
            DamageType = Models.Combat.DamageType.Physical;
            DropTable = DropTables.Parliament;
        }
    }
}
