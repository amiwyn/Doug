using Doug.Monsters.Droptables;

namespace Doug.Monsters
{
    public class Mario : Monster
    {
        public const string MonsterId = "mario";

        public Mario()
        {
            Id = MonsterId;
            Name = "Mario";
            Description = "Father of jimmy neutron. Also a good teacher";
            Image = "https://www2.ift.ulaval.ca/~mmarchand/mmarchand.jpg";
            Level = 60;
            ExperienceValue = 680;
            MaxHealth = Health = 3800;
            MinAttack = 698;
            MaxAttack = 1098;
            Hitrate = 93;
            Dodge = 72;
            Defense = 154;
            Resistance = 39;
            AttackCooldown = 20;
            DamageType = Models.Combat.DamageType.Magical;
            DropTable = DropTables.University;
        }
    }
}
