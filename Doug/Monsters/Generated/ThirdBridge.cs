using Doug.Monsters.Droptables;

namespace Doug.Monsters
{
    public class ThirdBridge : Monster
    {
        public const string MonsterId = "bridge";

        public ThirdBridge()
        {
            Id = MonsterId;
            Name = "Third bridge";
            Description = "This thing is a legend.";
            Image = "http://img.src.ca/2016/09/14/635x357/160914_6v08w_lien-tunnel_sn635.jpg";
            Level = 68;
            ExperienceValue = 744;
            MaxHealth = Health = 4824;
            MinAttack = 883;
            MaxAttack = 1385;
            Hitrate = 104;
            Dodge = 81;
            Defense = 195;
            Resistance = 41;
            AttackCooldown = 20;
            DamageType = Models.Combat.DamageType.Magical;
            DropTable = StRochTable.Drops;
        }
    }
}
