using System;
using System.Linq;
using Doug.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace WebJobs
{
    class Program
    {
        static void Main()
        {
            var connectionString = Environment.GetEnvironmentVariable("dougbotdb");

            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .AddDbContext<DougContext>(options => options.UseSqlServer(connectionString ?? throw new InvalidOperationException()))
                .BuildServiceProvider();

            var db = serviceProvider.GetService<DougContext>();
            AddCreditsToRuler(db);

            db.SaveChanges();
        }

        private static void AddCreditsToRuler(DougContext db)
        {
            var government = db.Government.Single();

            var ruler = db.Users.Single(usr => usr.Id == government.Ruler);
            ruler.Credits += 100;
        }
    }
}
