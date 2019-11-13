using Doug.Monsters.Droptables;

namespace Doug.Monsters
{
    public class PiquerieManager : Monster
    {
        public const string MonsterId = "piquerie";

        public PiquerieManager()
        {
            Id = MonsterId;
            Name = "Piquerie manager";
            Description = "The manager of the piquerie on the roof of the parking";
            Image = "https://previews.123rf.com/images/vadimgozhda/vadimgozhda1503/vadimgozhda150300892/37257983-handsome-businessman-is-working-with-laptop-in-office-is-looking-at-the-camera-.jpg";
            Level = 10;
            ExperienceValue = 280;
            MaxHealth = Health = 300;
            MinAttack = 68;
            MaxAttack = 118;
            Hitrate = 22;
            Dodge = 17;
            Defense = 14;
            Resistance = 22;
            AttackCooldown = 30;
            DamageType = Models.Combat.DamageType.Magical;
            DropTable = DropTables.StRoch;
        }
    }
}
