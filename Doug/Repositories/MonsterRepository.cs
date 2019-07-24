using System.Collections.Generic;
using System.Linq;
using Doug.Models;
using Doug.Monsters;

namespace Doug.Repositories
{
    public interface IMonsterRepository
    {
        IEnumerable<Monster> GetMonsters();
    }

    public class MonsterRepository : IMonsterRepository
    {
        private readonly DougContext _db;

        public MonsterRepository(DougContext db)
        {
            _db = db;
        }

        public IEnumerable<Monster> GetMonsters()
        {
            return _db.Monsters.ToList();
        }
    }
}
