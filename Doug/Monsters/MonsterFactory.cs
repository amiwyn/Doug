using System;
using System.Collections.Generic;
using Doug.Monsters.Brigands;
using Doug.Monsters.Seagulls;


namespace Doug.Monsters
{
    public interface IMonsterFactory
    {
        Monster CreateMonster(string monsterId);
    }

    public class MonsterFactory : IMonsterFactory
    {
        private readonly Dictionary<string, Func<Monster>> _monsters;

        public MonsterFactory()
        {
            _monsters = new Dictionary<string, Func<Monster>> 
            {
                { Seagull.MonsterId, () => new Seagull() },
                { Biker.MonsterId, () => new Biker() },
                { Hobo.MonsterId, () => new Hobo() },
                { Gangster.MonsterId, () => new Gangster() },
                { Codeboxx.MonsterId, () => new Codeboxx() },
                { WhiteBitch.MonsterId, () => new WhiteBitch() }
            };
        }

        public Monster CreateMonster(string monsterId)
        {
            return _monsters.GetValueOrDefault(monsterId)();
        }
    }
}
