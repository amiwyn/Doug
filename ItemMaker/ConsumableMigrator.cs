using System.IO;
using System.Linq;
using Doug;
using Doug.Items;

namespace ItemMigrator
{
    class ConsumableMigrator : Migrator
    {
        public void Migrate(DougContext db)
        {
            var lines = File.ReadLines("consumables.csv").ToList();
            lines.RemoveAt(0);
            var items = lines.Select(CreateEntity);

            foreach (var item in items)
            {
                if (!db.Items.Any(itm => itm.Id == item.Id))
                {
                    db.Items.Add(item);
                }
            }
            db.SaveChanges();
        }

        private Consumable CreateEntity(string line)
        {
            var values = Split(line);

            //Id,Name,Rarity,Icon,Price,Tradable,Sellable,Action,Target Action,Description
            return new Consumable
            {
                Id = values[0],
                Name = values[1],
                Rarity = (Rarity) int.Parse(values[2]),
                Icon = CreateIcon(values[3]),
                Price = int.Parse(values[4]),
                IsTradable = values[5] == "1",
                IsSellable = values[6] == "1",
                ActionId = values[7],
                TargetActionId = values[8],
                Description = values[9]
            };
        }
    }
}
