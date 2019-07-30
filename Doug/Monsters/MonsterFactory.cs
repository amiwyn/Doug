using System;
using System.Collections.Generic;
using System.Linq;
using Doug.Monsters.Brigands;
using Doug.Monsters.Seagulls;

namespace Doug.Monsters
{
    public interface IMonsterFactory
    {
        Monster CreateMonster(string monsterId);
        Monster CreateRandomMonster(Random random);
    }

    public class MonsterFactory : IMonsterFactory
    {
        private Dictionary<string, Func<Monster>> _monsters;

        public MonsterFactory()
        {
            _monsters = new Dictionary<string, Func<Monster>> 
            {
                { Seagull.MonsterId, () => new Seagull() },
                { Biker.MonsterId, () => new Biker() }
            };
        }

        public Monster CreateMonster(string monsterId)
        {
            return _monsters.GetValueOrDefault(monsterId)();
        }

        public Monster CreateRandomMonster(Random random)
        {
            var list = _monsters.ToList();
            var index = random.Next(0, list.Count);
            return list.ElementAt(index).Value();
        }
    }
}
