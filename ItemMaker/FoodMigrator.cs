using System.IO;
using System.Linq;
using Doug;
using Doug.Items;

namespace ItemMigrator
{
    class FoodMigrator : Migrator
    {
        public void Migrate(DougContext db)
        {
            var lines = File.ReadLines("food.csv").ToList();
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

        private Food CreateEntity(string line)
        {
            var values = Split(line);

            if (!string.IsNullOrWhiteSpace(values[9]))
            {
                return new SpecialFood()
                {
                    Id = values[0],
                    Name = values[1],
                    Rarity = (Rarity)int.Parse(values[2]),
                    Icon = CreateIcon(values[3]),
                    Price = int.Parse(values[4]),
                    IsTradable = values[5] == "1",
                    IsSellable = values[6] == "1",
                    HealthAmount = int.Parse(values[7]),
                    EnergyAmount = int.Parse(values[8]),
                    EffectId = values[9],
                    Duration = int.Parse(values[10]),
                    Description = values[11]
                };
            }

            return new Food
            {
                Id = values[0],
                Name = values[1],
                Rarity = (Rarity) int.Parse(values[2]),
                Icon = CreateIcon(values[3]),
                Price = int.Parse(values[4]),
                IsTradable = values[5] == "1",
                IsSellable = values[6] == "1",
                HealthAmount = int.Parse(values[7]),
                EnergyAmount = int.Parse(values[8]),
                Description = values[11]
            };
        }
    }
}
