using System.Collections.Generic;
using Doug.Items.Consumables;
using Doug.Items.Misc.Drops;
using Doug.Models.Monsters;

namespace Doug.Monsters.Brigands
{
    public class Codeboxx : Monster
    {
        public const string MonsterId = "codeboxx";

        public Codeboxx()
        {
            Id = MonsterId;
            Name = "Codeboxx Pleb";
            Description = "He's actually good at making coffee!";
            Image = "https://i.imgur.com/GjPJeVX.jpg";
            Level = 5;
            ExperienceValue = 150;

            MaxHealth = Health = 500;
            MinAttack = 20;
            MaxAttack = 40;
            Hitrate = 5;
            Dodge = 20;
            Defense = 18;
            Resistance = 8;

            DropTable = new Dictionary<LootItem, double>
            {
                { new LootItem(new CoffeeCup(), 1), 0.05 },
                { new LootItem(new IronIngot(), 1), 0.15 },
            };
        }
    }
}
