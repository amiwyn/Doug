using System.IO;
using System.Linq;
using Doug;
using Doug.Items;

namespace ItemMigrator
{
    class TicketsMigrator : Migrator
    {
        public void Migrate(DougContext db)
        {
            var lines = File.ReadLines("tickets.csv").ToList();
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

        private Ticket CreateEntity(string line)
        {
            var values = Split(line);

            return new Ticket
            {
                Id = values[0],
                Name = values[1],
                Rarity = (Rarity) int.Parse(values[2]),
                Icon = CreateIcon(values[3]),
                Price = int.Parse(values[4]),
                Channel = values[5],
                Description = values[6]
            };
        }
    }
}
