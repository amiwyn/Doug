using System.Collections.Generic;
using Doug.Items.Consumables;
using Doug.Items.Misc.Drops;
using Doug.Models;

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
            ExperienceValue = 650;

            MaxHealth = Health = 666;
            MinAttack = 98;
            MaxAttack = 139;
            Hitrate = 48;
            Dodge = 48;
            Defense = 32;
            Resistance = 10;

            DropTable = new Dictionary<LootItem, double>
            {
                { new LootItem(CoffeeCup.ItemId, 1), 0.05 },
                { new LootItem(IronIngot.ItemId, 1), 0.15 },
                { new LootItem(BikerCocaine.ItemId, 1), 0.3 }
            };
        }
    }
}
