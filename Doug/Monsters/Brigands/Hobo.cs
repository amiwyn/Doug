using Doug.Items.Consumables;
using Doug.Items.Misc.Drops;
using Doug.Models;
using System.Collections.Generic;

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
            Image = "https://i.imgur.com/PK51VpL.jpg";
            Level = 8;
            ExperienceValue = 100;

            MaxHealth = Health = 269;
            MinAttack = 69;
            MaxAttack = 69;
            Hitrate = 7;
            Dodge = 18;
            Defense = 17;
            Resistance = 9;

            DropTable = new Dictionary<LootItem, double>
            {
                { new LootItem(new Apple(), 1), 0.05 },
                { new LootItem(new CoffeeCup(), 1), 0.5 },
                { new LootItem(new IronIngot(), 1), 0.1 }
            };
        }
    }
}
