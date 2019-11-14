using Doug.Monsters.Droptables;

namespace Doug.Monsters
{
    public class Kim : Monster
    {
        public const string MonsterId = "kim";

        public Kim()
        {
            Id = MonsterId;
            Name = "Kim";
            Description = "The leader of the best Korea.";
            Image = "https://cdn.vox-cdn.com/thumbor/VXcdncaX04O11BGJv8oF-pBYv1Q=/0x0:1500x844/1200x675/filters:focal(630x302:870x542)/cdn.vox-cdn.com/uploads/chorus_image/image/63988749/kim_jon_world_lead.0.jpg";
            Level = 46;
            ExperienceValue = 568;
            MaxHealth = Health = 2316;
            MinAttack = 431;
            MaxAttack = 683;
            Hitrate = 73;
            Dodge = 57;
            Defense = 95;
            Resistance = 36;
            AttackCooldown = 20;
            DamageType = Models.Combat.DamageType.Physical;
            DropTable = DropTables.Japan;
        }
    }
}
