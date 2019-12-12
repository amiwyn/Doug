using System;
using System.Net.Http;
using Doug;
using Doug.Repositories;
using Doug.Services;
using Microsoft.Azure.WebJobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Functions
{
    public static class GameUpdate
    {
        [FunctionName("GameUpdate")]
        public static void Run([TimerTrigger("0 */5 * * * *")]TimerInfo timer, ILogger log)
        {
            log.LogInformation($"Game update executed at: {DateTime.Now}");

            var connectionString = Environment.GetEnvironmentVariable("dougbotdb");

            var services = new ServiceCollection();
            services.AddDbContext<DougContext>(options =>
                options.UseSqlServer(connectionString ?? throw new InvalidOperationException()));
            services.AddSingleton(new HttpClient(new HttpClientHandler(), false));
            Startup.RegisterDougServices(services);
            var serviceProvider = services.BuildServiceProvider();

            var monsterService = serviceProvider.GetService<IMonsterService>();
            var userRepository = serviceProvider.GetService<IUserRepository>();

            monsterService.RollMonsterSpawn();
            userRepository.RegenerateUsers();
        }
    }
}
