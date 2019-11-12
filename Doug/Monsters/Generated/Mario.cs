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
            Level = 58;
            ExperienceValue = 664;
            MaxHealth = Health = 3564;
            MinAttack = 656;
            MaxAttack = 1032;
            Hitrate = 90;
            Dodge = 70;
            Defense = 145;
            Resistance = 39;
            AttackCooldown = 20;
            DamageType = Models.Combat.DamageType.Magical;
            DropTable = StRochTable.Drops;
        }
    }
}
