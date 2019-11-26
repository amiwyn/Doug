using System.IO;
using System.Linq;
using Doug;
using Doug.Items;

namespace ItemMigrator
{
    class SkillbookMigrator : Migrator
    {
        public void Migrate(DougContext db)
        {
            var lines = File.ReadLines("skillbooks.csv").ToList();
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

        private SkillBook CreateEntity(string line)
        {
            var values = Split(line);

            return new SkillBook
            {
                Name = values[0],
                Id = values[1],
                Rarity = (Rarity) int.Parse(values[2]),
                Icon = CreateIcon(values[3]),
                Price = int.Parse(values[4]),
                LevelRequirement = int.Parse(values[5]),
                StrengthRequirement = int.Parse(values[6]),
                AgilityRequirement = int.Parse(values[7]),
                IntelligenceRequirement = int.Parse(values[8]),
                SkillId = values[9],
                Description = values[10]
            };
        }
    }
}
