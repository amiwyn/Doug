using System.Collections.Generic;
using Doug.Items.Consumables;
using Doug.Items.Misc.Drops;
using Doug.Models.Monsters;

namespace Doug.Monsters.Brigands
{
    public class Biker : Monster
    {
        public const string MonsterId = "biker";

        public Biker()
        {
            Id = MonsterId;
            Name = "Pont Rouge Biker";
            Description = "Attention les fifs quand la nuit tombe, y deviens un loup. C'tun Biker, Pont Rouge Panache.";
            Image = "https://i1.sndcdn.com/artworks-000008396349-n1h5ec-t500x500.jpg";
            Level = 15;
            ExperienceValue = 850;

            MaxHealth = Health = 666;
            MinAttack = 98;
            MaxAttack = 139;
            Hitrate = 12;
            Dodge = 48;
            Defense = 32;
            Resistance = 10;

            DropTable = new Dictionary<LootItem, double>
            {
                { new LootItem(new CoffeeCup(), 1), 0.05 },
                { new LootItem(new IronIngot(), 1), 0.15 },
                { new LootItem(new BikerCocaine(), 1), 0.3 }
            };
        }
    }
}
