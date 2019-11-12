using Doug.Monsters.Droptables;

namespace Doug.Monsters
{
    public class Francine : Monster
    {
        public const string MonsterId = "francine";

        public Francine()
        {
            Id = MonsterId;
            Name = "Francine";
            Description = "Studies very hard. Good girl.";
            Image = "https://media.licdn.com/dms/image/C4E03AQGCxb9uYK7bZA/profile-displayphoto-shrink_200_200/0?e=1579132800&v=beta&t=VVH0NIR0tUj2XGmoe63qT1Vanm_RtYO01R7JMjcx1dQ";
            Level = 56;
            ExperienceValue = 648;
            MaxHealth = Health = 3336;
            MinAttack = 615;
            MaxAttack = 969;
            Hitrate = 87;
            Dodge = 68;
            Defense = 136;
            Resistance = 38;
            AttackCooldown = 20;
            DamageType = Models.Combat.DamageType.Physical;
            DropTable = StRochTable.Drops;
        }
    }
}
