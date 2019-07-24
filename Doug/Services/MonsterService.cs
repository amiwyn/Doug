using System;
using Doug.Monsters.Seagulls;
using Doug.Repositories;

namespace Doug.Services
{
    public interface IMonsterService
    {
        void RollMonsterSpawn();
    }

    public class MonsterService : IMonsterService
    {
        private const double SpawnChance = 0.2;

        private readonly IMonsterRepository _monsterRepository;

        public MonsterService(IMonsterRepository monsterRepository)
        {
            _monsterRepository = monsterRepository;
        }

        public void RollMonsterSpawn()
        {
            if (new Random().NextDouble() >= SpawnChance)
            {
                return;
            }

            _monsterRepository.SpawnMonster(new Seagull()); //TODO add more monster variety
        }
    }
}
