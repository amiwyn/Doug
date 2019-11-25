using System;
using System.IO;
using System.Linq;
using Doug;
using Doug.Items;

namespace ItemMigrator
{
    class ArmorMigrator : Migrator
    {
        public void Migrate(DougContext db, string path)
        {
            var lines = File.ReadLines(path).ToList();
            lines.RemoveAt(0);
            var items = lines.Select(CreateEntity);

            foreach (var item in items)
            {
                if (!db.Items.Any(itm => itm.Id == item.Id))
                {
                    db.Items.Add(item);
                }
                else
                {
                    db.Entry(db.Items.Find(item.Id)).CurrentValues.SetValues(item);
                }
            }
            db.SaveChanges();
        }

        private Item CreateEntity(string line)
        {
            var values = Split(line).Select(value => string.IsNullOrEmpty(value) ? "0" : value).ToList();
            Enum.TryParse(values[2], out EquipmentSlot slot);

            //Name,Level,Slot,Agi Req.,Int Req.,Str Req.,Price,Health,Mana,Max Atk.,Min Atk.,Atk. Spd,Hitrate,Dodge,Defense,Resistance,Health Regen,Mana Regen,Luk,Agi,Str,Con,Int,Icon,Id,Description
            return new EquipmentItem
            {
                Name = values[0],
                LevelRequirement = int.Parse(values[1]),
                Slot = slot,
                AgilityRequirement = int.Parse(values[3]),
                IntelligenceRequirement = int.Parse(values[4]),
                StrengthRequirement = int.Parse(values[5]),
                Price = int.Parse(values[6]),
                Health = int.Parse(values[7]),
                Energy = int.Parse(values[8]),
                MinAttack = int.Parse(values[9]),
                MaxAttack = int.Parse(values[10]),
                AttackSpeed = int.Parse(values[11]),
                Hitrate = int.Parse(values[12]),
                Dodge = int.Parse(values[13]),
                Defense = int.Parse(values[14]),
                Resistance = int.Parse(values[15]),
                HealthRegen = int.Parse(values[16]),
                EnergyRegen = int.Parse(values[17]),
                Luck = int.Parse(values[18]),
                Agility = int.Parse(values[19]),
                Strength = int.Parse(values[20]),
                Constitution = int.Parse(values[21]),
                Intelligence = int.Parse(values[22]),
                Icon = CreateIcon(values[23]),
                Id = values[24],
                Description = values[25]
            };
        }
    }
}
