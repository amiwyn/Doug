using Doug.Monsters.Droptables;

namespace Doug.Monsters
{
    public class ForeinStudent : Monster
    {
        public const string MonsterId = "student";

        public ForeinStudent()
        {
            Id = MonsterId;
            Name = "Forein student";
            Description = "A foreign student.";
            Image = "https://static3.bigstockphoto.com/4/7/1/large1500/174901975.jpg";
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
            DamageType = Models.Combat.DamageType.Physical;
            DropTable = DropTables.University;
        }
    }
}
