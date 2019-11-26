using System.IO;
using System.Linq;
using Doug;
using Doug.Items;

namespace ItemMigrator
{
    class ItemsMigrator : Migrator
    {
        public void Migrate(DougContext db)
        {
            var lines = File.ReadLines("items.csv").ToList();
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

        private Item CreateEntity(string line)
        {
            var values = Split(line);

            return new Item
            {
                Id = values[0],
                Name = values[1],
                Rarity = (Rarity) int.Parse(values[2]),
                Icon = CreateIcon(values[3]),
                Price = int.Parse(values[4]),
                IsTradable = values[5] == "1",
                IsSellable = values[6] == "1",
                Description = values[7]
            };
        }
    }
}
