using System.Collections.Generic;
using Doug.Items.Consumables;
using Doug.Items.Misc.Drops;
using Doug.Models;

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
            ExperienceValue = 50;

            MaxHealth = Health = 480;
            MinAttack = 78;
            MaxAttack = 96;
            Hitrate = 18;
            Dodge = 18;
            Defense = 20;
            Resistance = 10;

            DropTable = new Dictionary<LootItem, double>
            {
                { new LootItem(Apple.ItemId, 1), 0.05 },
                { new LootItem(GullFeather.ItemId, 1), 0.4 },
                { new LootItem(SharpBeak.ItemId, 1), 0.3 },
                { new LootItem(IronIngot.ItemId, 1), 0.1 }
            };
        }
    }
}
