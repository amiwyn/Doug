using System;
using System.Net.Http;
using Doug;
using Doug.Models;
using Doug.Repositories;
using Doug.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GameUpdateWebjob
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
            var userRepository = serviceProvider.GetService<IUserRepository>();

            monsterService.RollMonsterSpawn();
            userRepository.RegenerateUsers();
        }
    }
}
