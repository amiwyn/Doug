using Doug.Monsters.Droptables;

namespace Doug.Monsters
{
    public class Waifu : Monster
    {
        public const string MonsterId = "waifu";

        public Waifu()
        {
            Id = MonsterId;
            Name = "Waifu";
            Description = "This man will do everything to protect his wife.";
            Image = "https://cdn.techinasia.com/wp-content/uploads/2015/08/ItaspoTemple.jpg";
            Level = 48;
            ExperienceValue = 584;
            MaxHealth = Health = 2504;
            MinAttack = 465;
            MaxAttack = 736;
            Hitrate = 76;
            Dodge = 59;
            Defense = 103;
            Resistance = 36;
            AttackCooldown = 20;
            DamageType = Models.Combat.DamageType.Physical;
            DropTable = DropTables.Japan;
        }
    }
}
