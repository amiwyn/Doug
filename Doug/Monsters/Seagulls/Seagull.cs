using Doug.Monsters.Droptables;

namespace Doug.Monsters.Seagulls
{
    public class Seagull : Monster
    {
        public const string MonsterId = "seagull";

        public Seagull()
        {
            Id = MonsterId;
            Name = "Seagull";
            Description = "A fierce animal with a razor sharp beak.";
            Image = "https://upload.wikimedia.org/wikipedia/commons/f/fb/Seagull_in_flight_by_Jiyang_Chen.jpg";
            Level = 10;
            ExperienceValue = 250;

            MaxHealth = Health = 480;
            MinAttack = 78;
            MaxAttack = 96;
            Hitrate = 6;
            Dodge = 18;
            Defense = 20;
            Resistance = 10;

            DropTable = StRochTable.Drops;
        }
    }
}
