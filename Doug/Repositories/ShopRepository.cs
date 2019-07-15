using System.Linq;
using Doug.Models;
using Microsoft.EntityFrameworkCore;

namespace Doug.Repositories
{
    public interface IShopRepository
    {
        Shop GetShop(string shopId);
    }

    public class ShopRepository : IShopRepository
    {
        private readonly DougContext _db;

        public ShopRepository(DougContext db)
        {
            _db = db;
        }

        public Shop GetShop(string shopId)
        {
            return _db.Shops.Include(shop => shop.ShopItems).SingleOrDefault(shop => shop.Id == shopId);
        }
    }
}
