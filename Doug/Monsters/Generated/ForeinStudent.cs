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
            DamageType = Models.Combat.DamageType.Physical;
            DropTable = StRochTable.Drops;
        }
    }
}
