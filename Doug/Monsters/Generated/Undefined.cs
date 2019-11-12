using Doug.Monsters.Droptables;

namespace Doug.Monsters
{
    public class Undefined : Monster
    {
        public const string MonsterId = "dess";

        public Undefined()
        {
            Id = MonsterId;
            Name = "undefined";
            Description = "undefined";
            Image = "https://i.imgur.com/2Azu5DJ.jpg";
            Level = 38;
            ExperienceValue = 504;
            MaxHealth = Health = 1644;
            MinAttack = 310;
            MaxAttack = 495;
            Hitrate = 61;
            Dodge = 48;
            Defense = 68;
            Resistance = 34;
            AttackCooldown = 20;
            DamageType = Models.Combat.DamageType.Magical;
            DropTable = StRochTable.Drops;
        }
    }
}
