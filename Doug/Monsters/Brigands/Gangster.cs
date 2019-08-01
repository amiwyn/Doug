using System.Collections.Generic;
using Doug.Items.Consumables;
using Doug.Items.Misc.Drops;
using Doug.Models;

namespace Doug.Monsters.Brigands
{
    public class Gangster : Monster
    {
        public const string MonsterId = "gangster";

        public Gangster()
        {
            Id = MonsterId;
            Name = "Sorel-Tracy Gang Member";
            Description = "Ma te crisser un coup de pied dans gorge.";
            Image = "https://i.imgur.com/mVUCtB7.jpg";
            Level = 10;
            ExperienceValue = 500;

            MaxHealth = Health = 350;
            MinAttack = 50;
            MaxAttack = 100;
            Hitrate = 38;
            Dodge = 28;
            Defense = 22;
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
