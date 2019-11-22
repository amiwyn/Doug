using System.Linq;
using Doug.Models.Monsters;
using Microsoft.EntityFrameworkCore;

namespace Doug.Repositories
{
    public interface IMonsterRepository
    {
        Monster GetMonster(string monsterId);
    }

    public class MonsterRepository : IMonsterRepository
    {
        private readonly DougContext _db;

        public MonsterRepository(DougContext db)
        {
            _db = db;
        }

        public Monster GetMonster(string monsterId)
        {
            return _db.Monsters.Include(monster => monster.DropTable).Single(monster => monster.Id == monsterId);
        }
    }
}
