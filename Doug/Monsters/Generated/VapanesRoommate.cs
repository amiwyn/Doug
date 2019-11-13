using Doug.Monsters.Droptables;

namespace Doug.Monsters
{
    public class VapanesRoommate : Monster
    {
        public const string MonsterId = "vap_roommate";

        public VapanesRoommate()
        {
            Id = MonsterId;
            Name = "Vapane's roommate";
            Description = "He sells shiny little golden cowboy hats.";
            Image = "https://i.imgur.com/woaZr1E.jpg";
            Level = 12;
            ExperienceValue = 296;
            MaxHealth = Health = 344;
            MinAttack = 76;
            MaxAttack = 131;
            Hitrate = 25;
            Dodge = 20;
            Defense = 16;
            Resistance = 23;
            AttackCooldown = 30;
            DamageType = Models.Combat.DamageType.Physical;
            DropTable = DropTables.Vanier;
        }
    }
}
