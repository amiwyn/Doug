using System;
using System.IO;
using System.Linq;
using Doug;
using Doug.Items;
using Doug.Models;
using Microsoft.EntityFrameworkCore;

namespace ItemMigrator
{
    class ShopsMigrator : Migrator
    {
        public void Migrate(DougContext db)
        {
            var lines = File.ReadLines("shops.csv").ToList();
            lines.RemoveAt(0);
            var items = lines.Select(CreateEntity).ToList();

            var shops = db.Shops.Include(shp => shp.ShopItems).ToList();

            foreach (var shop in shops)
            {
                var shopItems = items.Where(itm => itm.ShopId == shop.Id).ToList();

                if (!shopItems.Any())
                {
                    continue;
                }

                shop.ShopItems = shopItems;
            }
            db.SaveChanges();
        }

        private ShopItem CreateEntity(string line)
        {
            var values = Split(line).Select(value => string.IsNullOrEmpty(value) ? "0" : value).ToList();

            return new ShopItem
            {
                ItemId = values[0],
                ShopId = values[1]
            };
        }
    }
}
