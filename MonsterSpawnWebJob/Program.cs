using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Doug;
using Doug.Models;
using Doug.Monsters;
using Doug.Repositories;
using Doug.Services;
using Doug.Slack;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MonsterSpawnWebJob
{
    class Program
    {
        private const double SpawnChance = 0.2;
        private const string PvpChannel = "CL2TYGE1E";
        private const int MaximumMonsterTypeInChannel = 3;

        static void Main()
        {
            var connectionString = Environment.GetEnvironmentVariable("dougbotdb");

            var services = new ServiceCollection();
            services.AddDbContext<DougContext>(options =>
                options.UseSqlServer(connectionString ?? throw new InvalidOperationException()));
            services.AddSingleton(new HttpClient(new HttpClientHandler(), false));
            Startup.RegisterDougServices(services);
            var serviceProvider = services.BuildServiceProvider();

            var channelRepository = serviceProvider.GetService<IChannelRepository>();
            var monsterFactory = serviceProvider.GetService<IMonsterFactory>();
            var monsterRepository = serviceProvider.GetService<IMonsterRepository>();
            var slack = serviceProvider.GetService<ISlackWebApi>();

            RollMonsterSpawn(channelRepository, monsterFactory, monsterRepository, slack);
        }

        private static void RollMonsterSpawn(IChannelRepository channelRepository, IMonsterFactory monsterFactory,
            IMonsterRepository monsterRepository, ISlackWebApi slack)
        {
            var random = new Random();
            if (random.NextDouble() >= SpawnChance)
            {
                return;
            }

            var channel = PvpChannel;
            if (random.Next(2) == 0)
            {
                channel = PickRandomChannel(random, channelRepository).Id;
            }

            var monster = monsterFactory.CreateRandomMonster(random);
            var monstersInChannel = monsterRepository.GetMonsters(channel);

            if (monstersInChannel.Count(monsta => monsta.MonsterId == monster.Id) >= MaximumMonsterTypeInChannel)
            {
                return;
            }

            monsterRepository.SpawnMonster(monster, channel);
            slack.BroadcastMessage(string.Format(DougMessages.MonsterSpawned, monster.Name), channel).Wait();
        }

        private static Channel PickRandomChannel(Random random, IChannelRepository channelRepository)
        {
            var channels = channelRepository.GetChannels().ToList();
            var index = random.Next(0, channels.Count);
            return channels.ElementAt(index);
        }
    }
}
