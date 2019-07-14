using System.Linq;
using Doug.Models;

namespace Doug.Repositories
{
    public interface IShopRepository
    {
        Shop GetShop(string shopId);
        Shop GetDefaultShop();
    }

    public class ShopRepository : IShopRepository
    {
        public const string DefaultShopId = "default";

        private readonly DougContext _db;

        public ShopRepository(DougContext db)
        {
            _db = db;
        }

        public Shop GetShop(string shopId)
        {
            return _db.Shops.SingleOrDefault(shop => shop.Id == shopId);
        }

        public Shop GetDefaultShop()
        {
            return _db.Shops.Single(shop => shop.Id == DefaultShopId);
        }
    }
}
