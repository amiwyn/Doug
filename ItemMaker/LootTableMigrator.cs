using System.IO;
using System.Linq;
using Doug;
using Doug.Models;
using Microsoft.EntityFrameworkCore;

namespace ItemMigrator
{
    class LootTableMigrator : Migrator
    {
        public void Migrate(DougContext db)
        {
            var lines = File.ReadLines("droptables.csv").ToList();
            lines.RemoveAt(0);

            foreach (var line in lines)
            {
                var values = Split(line);
                var existingTable = db.Droptables.Include(t => t.Items).SingleOrDefault(table => table.Id == values[0]);
                if (existingTable != null)
                {
                    if (existingTable.Items.Any(itm => itm.Id == values[1]))
                    {
                        continue;
                    }

                    existingTable.Items.Add(new LootItem(values[1], int.Parse(values[2]), double.Parse(values[3])));
                }
                else
                {
                    var droptable = new DropTable() { Id = values[0]};
                    droptable.Items.Add(new LootItem(values[1], int.Parse(values[2]), double.Parse(values[3])));
                    db.Droptables.Add(droptable);
                }
                db.SaveChanges();
            }
        }
    }
}
