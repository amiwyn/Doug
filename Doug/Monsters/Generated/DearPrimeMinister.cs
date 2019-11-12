using Doug.Monsters.Droptables;

namespace Doug.Monsters
{
    public class DearPrimeMinister : Monster
    {
        public const string MonsterId = "legault";

        public DearPrimeMinister()
        {
            Id = MonsterId;
            Name = "Dear prime minister";
            Description = "Oops he did it again.";
            Image = "https://journalmetro.com/wp-content/uploads/2019/10/Francois-legault02.jpg?w=860";
            Level = 26;
            ExperienceValue = 408;
            MaxHealth = Health = 876;
            MinAttack = 172;
            MaxAttack = 280;
            Hitrate = 44;
            Dodge = 35;
            Defense = 38;
            Resistance = 30;
            AttackCooldown = 30;
            DamageType = Models.Combat.DamageType.Physical;
            DropTable = StRochTable.Drops;
        }
    }
}
