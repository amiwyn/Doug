using Doug.Monsters.Droptables;

namespace Doug.Monsters
{
    public class PontRougeBiker : Monster
    {
        public const string MonsterId = "biker";

        public PontRougeBiker()
        {
            Id = MonsterId;
            Name = "Pont-Rouge biker";
            Description = "Attention les fifs quand la nuit tombe y deviens un loup. C'tun Biker Pont Rouge Panache.";
            Image = "https://i1.sndcdn.com/artworks-000008396349-n1h5ec-t500x500.jpg";
            Level = 18;
            ExperienceValue = 344;
            MaxHealth = Health = 524;
            MinAttack = 109;
            MaxAttack = 181;
            Hitrate = 33;
            Dodge = 26;
            Defense = 23;
            Resistance = 26;
            AttackCooldown = 30;
            DamageType = Models.Combat.DamageType.Physical;
            DropTable = DropTables.Vanier;
        }
    }
}
