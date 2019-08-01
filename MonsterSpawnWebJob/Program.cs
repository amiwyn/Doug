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
        static void Main()
        {
            var connectionString = Environment.GetEnvironmentVariable("dougbotdb");

            var services = new ServiceCollection();
            services.AddDbContext<DougContext>(options =>
                options.UseSqlServer(connectionString ?? throw new InvalidOperationException()));
            services.AddSingleton(new HttpClient(new HttpClientHandler(), false));
            Startup.RegisterDougServices(services);
            var serviceProvider = services.BuildServiceProvider();

            var monsterService = serviceProvider.GetService<IMonsterService>();

            monsterService.RollMonsterSpawn();
        }
    }
}
