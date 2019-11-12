using Doug.Monsters.Droptables;

namespace Doug.Monsters
{
    public class Manon : Monster
    {
        public const string MonsterId = "manon";

        public Manon()
        {
            Id = MonsterId;
            Name = "Manon";
            Description = "As much personality as mustache.";
            Image = "https://instagram.fymy1-1.fna.fbcdn.net/vp/aafe1c1d3039e562d30d31c9d6f0e3cd/5E4DF903/t51.2885-15/e35/35166432_612229932495636_6164302788827283456_n.jpg?_nc_ht=instagram.fymy1-1.fna.fbcdn.net&_nc_cat=100";
            Level = 30;
            ExperienceValue = 440;
            MaxHealth = Health = 1100;
            MinAttack = 212;
            MaxAttack = 342;
            Hitrate = 50;
            Dodge = 39;
            Defense = 46;
            Resistance = 31;
            AttackCooldown = 20;
            DamageType = Models.Combat.DamageType.Magical;
            DropTable = StRochTable.Drops;
        }
    }
}
