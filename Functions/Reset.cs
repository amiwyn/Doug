using System;
using System.Linq;
using Doug;
using Microsoft.Azure.WebJobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Functions
{
    public static class Reset
    {
        [FunctionName("Reset")]
        public static void Run([TimerTrigger("0 0 1 * * *")]TimerInfo myTimer, ILogger log)
        {
            var connectionString = Environment.GetEnvironmentVariable("dougbotdb");

            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .AddDbContext<DougContext>(options => options.UseSqlServer(connectionString ?? throw new InvalidOperationException()))
                .BuildServiceProvider();

            var db = serviceProvider.GetService<DougContext>();
            AddCreditsToRuler(db);
            ResetRecentSlurs(db);

            db.SaveChanges();
        }

        private static void AddCreditsToRuler(DougContext db)
        {
            var government = db.Government.Single();

            var ruler = db.Users.Single(usr => usr.Id == government.Ruler);
            ruler.Credits += 2000;
        }

        private static void ResetRecentSlurs(DougContext db)
        {
            db.RecentSlurs.RemoveRange(db.RecentSlurs);
        }
    }
}
