using System;
using System.IO;
using System.Linq;
using Doug;
using Doug.Items;
using Doug.Items.WeaponType;

namespace ItemMigrator
{
    class SpecialWeaponMigrator : Migrator
    {
        public void Migrate(DougContext db)
        {
            var lines = File.ReadLines("specialweapons.csv").ToList();
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

        private Weapon CreateWeapon(string type)
        {
            switch (type)
            {
                case "Staff": return new Staff();
                case "Bow": return new Bow();
                case "Sword": return new Sword();
                case "Axe": return new Axe();
                case "Shield": return new Shield();
                case "Gun": return new Gun();
                case "GreatSword": return new GreatSword();
                case "Claws": return new Claws();
                default: throw new ArgumentException("bitch");
            }
        }

        private Item CreateEntity(string line)
        {
            var values = Split(line).Select(value => string.IsNullOrEmpty(value) ? "0" : value).ToList();

            var weapon = CreateWeapon(values[2]);
            weapon.Name = values[0];
            weapon.LevelRequirement = int.Parse(values[1]);
            weapon.AgilityRequirement = int.Parse(values[3]);
            weapon.IntelligenceRequirement = int.Parse(values[4]);
            weapon.StrengthRequirement = int.Parse(values[5]);
            weapon.Price = int.Parse(values[6]);
            weapon.Health = int.Parse(values[7]);
            weapon.Energy = int.Parse(values[8]);
            weapon.MinAttack = int.Parse(values[9]);
            weapon.MaxAttack = int.Parse(values[10]);
            weapon.Hitrate = int.Parse(values[12]);
            weapon.Dodge = int.Parse(values[13]);
            weapon.Defense = int.Parse(values[14]);
            weapon.Resistance = int.Parse(values[15]);
            weapon.HealthRegen = int.Parse(values[16]);
            weapon.EnergyRegen = int.Parse(values[17]);
            weapon.Luck = int.Parse(values[18]);
            weapon.Agility = int.Parse(values[19]);
            weapon.Strength = int.Parse(values[20]);
            weapon.Constitution = int.Parse(values[21]);
            weapon.Intelligence = int.Parse(values[22]);
            weapon.Rarity = (Rarity)int.Parse(values[23]);
            weapon.EffectId = values[24];
            weapon.IsSellable = values[25] == "1";
            weapon.IsTradable = values[26] == "1";
            weapon.Icon = CreateIcon(values[27]);
            weapon.Id = values[28];
            weapon.Description = values[29];
            weapon.LuckFactor = int.Parse(values[30]);
            weapon.AgilityFactor = int.Parse(values[31]);
            weapon.StrengthFactor = int.Parse(values[32]);
            weapon.ConstitutionFactor = int.Parse(values[33]);
            weapon.IntelligenceFactor = int.Parse(values[34]);
            weapon.HealthFactor = int.Parse(values[35]);
            weapon.EnergyFactor = int.Parse(values[36]);
            weapon.AttackSpeedFactor = int.Parse(values[37]);
            weapon.CriticalDamageFactor = int.Parse(values[38]);
            weapon.CriticalHitChanceFactor = int.Parse(values[39]);
            weapon.Pierce = int.Parse(values[40]);
            weapon.PierceFactor = int.Parse(values[41]);
            weapon.HitRateFactor = int.Parse(values[42]);
            weapon.DodgeFactor = int.Parse(values[43]);
            weapon.FlatEnergyRegen = int.Parse(values[44]);
            weapon.FlatHealthRegen = int.Parse(values[45]);

            return weapon;
        }
    }
}
