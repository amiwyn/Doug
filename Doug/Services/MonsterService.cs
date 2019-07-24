using System;
using Doug.Monsters.Seagulls;
using Doug.Repositories;
using Doug.Slack;

namespace Doug.Services
{
    public interface IMonsterService
    {
        void RollMonsterSpawn();
    }

    public class MonsterService : IMonsterService
    {
        private const double SpawnChance = 0.2;
        private const string PvpChannel = "CL2TYGE1E";

        private readonly IMonsterRepository _monsterRepository;
        private readonly ISlackWebApi _slack;

        public MonsterService(IMonsterRepository monsterRepository, ISlackWebApi slack)
        {
            _monsterRepository = monsterRepository;
            _slack = slack;
        }

        public void RollMonsterSpawn()
        {
            if (new Random().NextDouble() >= SpawnChance)
            {
                return;
            }

            var monster = new Seagull(); //TODO add more monster variety and pick them randomly (or based on present players levels)

            _monsterRepository.SpawnMonster(monster); 
            _slack.BroadcastMessage(string.Format(DougMessages.MonsterSpawned, monster.Name), PvpChannel);
        }
    }
}
