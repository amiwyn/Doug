using Doug.Monsters.Droptables;

namespace Doug.Monsters
{
    public class RogerRemblais : Monster
    {
        public const string MonsterId = "remblais";

        public RogerRemblais()
        {
            Id = MonsterId;
            Name = "Roger Remblais";
            Description = "J'ai l'doua";
            Image = "https://i.ytimg.com/vi/NIIFJ-AHPKE/hqdefault.jpg";
            Level = 34;
            ExperienceValue = 472;
            MaxHealth = Health = 1356;
            MinAttack = 259;
            MaxAttack = 414;
            Hitrate = 56;
            Dodge = 44;
            Defense = 57;
            Resistance = 32;
            AttackCooldown = 20;
            DamageType = Models.Combat.DamageType.Physical;
            DropTable = StRochTable.Drops;
        }
    }
}
