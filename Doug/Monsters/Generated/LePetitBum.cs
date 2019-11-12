using Doug.Monsters.Droptables;

namespace Doug.Monsters
{
    public class LePetitBum : Monster
    {
        public const string MonsterId = "yann";

        public LePetitBum()
        {
            Id = MonsterId;
            Name = "Le petit bum";
            Description = "Ma te crisser un coup de pied dans le gorge.";
            Image = "https://i.imgur.com/mVUCtB7.jpg";
            Level = 20;
            ExperienceValue = 360;
            MaxHealth = Health = 600;
            MinAttack = 122;
            MaxAttack = 202;
            Hitrate = 36;
            Dodge = 28;
            Defense = 26;
            Resistance = 27;
            AttackCooldown = 30;
            DamageType = Models.Combat.DamageType.Physical;
            DropTable = StRochTable.Drops;
        }
    }
}
