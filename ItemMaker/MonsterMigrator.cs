using System.IO;
using System.Linq;
using Doug;
using Doug.Models.Combat;
using Doug.Models.Monsters;

namespace ItemMigrator
{
    class MonsterMigrator : Migrator
    {
        public void Migrate(DougContext db)
        {
            var lines = File.ReadLines("monsters.csv").Where(line => line[0] != ',').ToList();
            lines.RemoveAt(0);
            var items = lines.Select(CreateEntity);

            foreach (var item in items)
            {
                if (!db.Items.Any(itm => itm.Id == item.Id))
                {
                    db.Monsters.Add(item);
                }
            }
            db.SaveChanges();
        }

        private Monster CreateEntity(string line)
        {
            var values = Split(line);

            return new Monster
            {
                Name = values[0],
                Level = int.Parse(values[1]),
                Id = values[2],
                ExperienceValue = int.Parse(values[3]),
                DamageType = (DamageType)int.Parse(values[4]),
                Health = int.Parse(values[5]),
                MinAttack = int.Parse(values[6]),
                MaxAttack = int.Parse(values[7]),
                AttackCooldown = int.Parse(values[8]),
                Hitrate = int.Parse(values[9]),
                Dodge = int.Parse(values[10]),
                Resistance = int.Parse(values[11]),
                Defense = int.Parse(values[12]),
                Region = values[13],
                DropTableId = values[14],
                Description = values[15],
                Image = values[16]
            };
        }
    }
}
