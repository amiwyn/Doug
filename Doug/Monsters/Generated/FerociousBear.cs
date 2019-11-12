using Doug.Monsters.Droptables;

namespace Doug.Monsters
{
    public class FerociousBear : Monster
    {
        public const string MonsterId = "bear";

        public FerociousBear()
        {
            Id = MonsterId;
            Name = "Ferocious bear";
            Description = "Oh that wasn't what I had in mind.";
            Image = "https://i.guim.co.uk/img/media/d55f75a9b226515ce2ce3da5b28a8ba99101ec94/0_0_3577_2146/master/3577.jpg?width=620&quality=85&auto=format&fit=max&s=d5e27bace9218ef3946211198fa1faf1";
            Level = 32;
            ExperienceValue = 456;
            MaxHealth = Health = 1224;
            MinAttack = 235;
            MaxAttack = 377;
            Hitrate = 53;
            Dodge = 42;
            Defense = 51;
            Resistance = 32;
            AttackCooldown = 20;
            DamageType = Models.Combat.DamageType.Physical;
            DropTable = StRochTable.Drops;
        }
    }
}
