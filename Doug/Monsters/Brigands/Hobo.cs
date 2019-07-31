using System.Collections.Generic;
using Doug.Items.Consumables;
using Doug.Items.Misc.Drops;
using Doug.Models;

namespace Doug.Monsters.Brigands
{
    public class Hobo : Monster
    {
        public const string MonsterId = "hobo";

        public Hobo()
        {
            Id = MonsterId;
            Name = "Hobo";
            Description = "Little vapoting punk. Watch out, he can spit on you !!";
            Image = "https://imgur.com/a/ovnZdsK";
            Level = 10;
            ExperienceValue = 100;

            MaxHealth = Health = 269;
            MinAttack = 69;
            MaxAttack = 69;
            Hitrate = 18;
            Dodge = 18;
            Defense = 17;
            Resistance = 9;

            DropTable = new Dictionary<LootItem, double>
            {
                { new LootItem(Apple.ItemId, 1), 0.05 },
                { new LootItem(CoffeeCup.ItemId, 3), 0.05 },
                { new LootItem(IronIngot.ItemId, 1), 0.1 }
            };
        }
    }
}
