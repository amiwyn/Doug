using System.Collections.Generic;
using Doug.Items.Consumables;
using Doug.Items.Misc;
using Doug.Models;

namespace Doug.Monsters.Seagulls
{
    public class Seagull : Monster
    {
        public const string MonsterId = "seagull";

        public Seagull()
        {
            Id = MonsterId;
            Name = "A Seagull";
            Description = "A fierce animal with a razor sharp beak.";
            Level = 5;
            ExperienceValue = 50;

            MaxHealth = 180;
            MinAttack = 28;
            MaxAttack = 42;
            Hitrate = 18;
            Dodge = 18;
            Defense = 20;
            Resistance = 10;

            DropTable = new Dictionary<LootItem, double>
            {
                { new LootItem(Apple.ItemId, 1), 0.05 },
                { new LootItem(CoffeeCup.ItemId, 1), 0.05 },
                { new LootItem(SuicidePill.ItemId, 1), 0.05 },
                { new LootItem(Cigarette.ItemId, 1), 0.05 },
                { new LootItem(BachelorsDegree.ItemId, 1), 0.05 }
            };
        }
    }
}
