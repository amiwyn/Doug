using System;
using System.Net.Http;
using Doug;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ItemMigrator
{
    class Program
    {
        static void Main()
        {
            var incompleteConnString = "Server=tcp:dougdata.database.windows.net,1433;Initial Catalog=doug;Persist Security Info=False;User ID={0};Password={1};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            var connectionString = string.Format(incompleteConnString, Environment.GetEnvironmentVariable("DB_USER"), Environment.GetEnvironmentVariable("DB_PASS"));

            var services = new ServiceCollection();
            services.AddDbContext<DougContext>(options => options.UseSqlServer(connectionString));
            services.AddSingleton(new HttpClient(new HttpClientHandler(), false));
            Startup.RegisterDougServices(services);
            var serviceProvider = services.BuildServiceProvider();

            new ItemsMigrator().Migrate(serviceProvider.GetService<DougContext>());
            new ConsumableMigrator().Migrate(serviceProvider.GetService<DougContext>());
            new FoodMigrator().Migrate(serviceProvider.GetService<DougContext>());
            new SkillbookMigrator().Migrate(serviceProvider.GetService<DougContext>());
            new TicketsMigrator().Migrate(serviceProvider.GetService<DougContext>());
            new LootTableMigrator().Migrate(serviceProvider.GetService<DougContext>());
            new MonsterMigrator().Migrate(serviceProvider.GetService<DougContext>());
            new WeaponMigrator().Migrate(serviceProvider.GetService<DougContext>());
            new SpecialWeaponMigrator().Migrate(serviceProvider.GetService<DougContext>());
            new SpecialArmorMigrator().Migrate(serviceProvider.GetService<DougContext>());

            new ArmorMigrator().Migrate(serviceProvider.GetService<DougContext>(), "mage.csv");
            new ArmorMigrator().Migrate(serviceProvider.GetService<DougContext>(), "warrior.csv");
            new ArmorMigrator().Migrate(serviceProvider.GetService<DougContext>(), "rogue.csv");

            new ShopsMigrator().Migrate(serviceProvider.GetService<DougContext>());
        }
    }
}
