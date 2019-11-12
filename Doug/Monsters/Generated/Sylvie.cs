using Doug.Monsters.Droptables;

namespace Doug.Monsters
{
    public class Sylvie : Monster
    {
        public const string MonsterId = "sylvie";

        public Sylvie()
        {
            Id = MonsterId;
            Name = "Sylvie";
            Description = "A beautiful woman that has a fanclub of weird grown ass tech consultants.";
            Image = "https://i.imgur.com/rWbA0f1.png";
            Level = 40;
            ExperienceValue = 520;
            MaxHealth = Health = 1800;
            MinAttack = 338;
            MaxAttack = 538;
            Hitrate = 64;
            Dodge = 50;
            Defense = 74;
            Resistance = 34;
            AttackCooldown = 20;
            DamageType = Models.Combat.DamageType.Magical;
            DropTable = StRochTable.Drops;
        }
    }
}
