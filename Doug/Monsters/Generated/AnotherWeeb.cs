using Doug.Monsters.Droptables;

namespace Doug.Monsters
{
    public class Kim : Monster
    {
        public const string MonsterId = "kim";

        public Kim()
        {
            Id = MonsterId;
            Name = "Kim";
            Description = "Kim! The ruler of the best Korea.";
            Image = "https://upload.wikimedia.org/wikipedia/commons/d/d9/Kim_Jong-un_IKS_2018.jpg";
            Level = 46;
            ExperienceValue = 568;
            MaxHealth = Health = 2316;
            MinAttack = 431;
            MaxAttack = 683;
            Hitrate = 73;
            Dodge = 57;
            Defense = 95;
            Resistance = 36;
            AttackCooldown = 20;
            DamageType = Models.Combat.DamageType.Physical;
            DropTable = StRochTable.Drops;
        }
    }
}
