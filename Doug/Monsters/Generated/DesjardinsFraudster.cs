using Doug.Monsters.Droptables;

namespace Doug.Monsters
{
    public class DesjardinsFraudster : Monster
    {
        public const string MonsterId = "fraudster";

        public DesjardinsFraudster()
        {
            Id = MonsterId;
            Name = "Desjardins fraudster";
            Description = "Good job dumbass.";
            Image = "https://i.cbc.ca/1.5183362.1568904787!/cpImage/httpImage/image.jpg_gen/derivatives/16x9_780/desjardins-president-and-ceo-guy-cormier.jpg";
            Level = 64;
            ExperienceValue = 712;
            MaxHealth = Health = 4296;
            MinAttack = 788;
            MaxAttack = 1237;
            Hitrate = 98;
            Dodge = 77;
            Defense = 174;
            Resistance = 40;
            AttackCooldown = 20;
            DamageType = Models.Combat.DamageType.Physical;
            DropTable = DropTables.Beauce;
        }
    }
}
