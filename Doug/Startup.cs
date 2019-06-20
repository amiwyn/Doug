using System;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Doug.Commands;
using Doug.Items;
using Doug.Models;
using Doug.Repositories;
using Doug.Services;
using Doug.Slack;
using Hangfire;
using Hangfire.SQLite;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;

namespace Doug
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(x =>
                {
                    x.SerializerSettings.ContractResolver = new DefaultContractResolver
                    {
                        NamingStrategy = new SnakeCaseNamingStrategy()
                    };
                });

            services.AddSingleton(new HttpClient(new HttpClientHandler(), false));

            services.AddTransient<IEventService, EventService>();
            services.AddTransient<ISlackWebApi, SlackWebApi>();

            services.AddTransient<IItemEventDispatcher, ItemEventDispatcher>();
            services.AddTransient<IAuthorizationService, AuthorizationService>();
            services.AddTransient<ICoffeeService, CoffeeService>();

            services.AddTransient<ICoffeeCommands, CoffeeCommands>();
            services.AddTransient<ISlursCommands, SlursCommands>();
            services.AddTransient<ICreditsCommands, CreditsCommands>();
            services.AddTransient<ICasinoCommands, CasinoCommands>();
            services.AddTransient<ICombatCommands, CombatCommands>();
            services.AddTransient<IInventoryCommands, InventoryCommands>();
            services.AddTransient<IStatsCommands, StatsCommands>();

            services.AddTransient<IChannelRepository, ChannelRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<ICoffeeRepository, CoffeeRepository>();
            services.AddTransient<ISlurRepository, SlurRepository>();

            var env = Environment.GetEnvironmentVariable("APP_ENV");

            if (env == "production" || env == "staging-like")
            {
                var connectionString = string.Format(Configuration.GetConnectionString("DougDb"), Environment.GetEnvironmentVariable("DB_USER"), Environment.GetEnvironmentVariable("DB_PASS"));

                if (Environment.GetEnvironmentVariable("APP_ENV") == "production")
                {
                    connectionString = Configuration.GetConnectionString("dougbotdb");
                }

                services.AddHangfire(config => config.UseSqlServerStorage(connectionString));
                services.AddHangfireServer();

                services.AddDbContext<DougContext>(options => options.UseSqlServer(connectionString));
            }
            else
            {
                services.AddHangfire(config => config.UseSQLiteStorage("Data Source=jobs.db;"));
                services.AddHangfireServer();

                services.AddDbContext<DougContext>(options => options.UseSqlite("Data Source=doug.db"));
            }

        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
                app.Use(RequestSigning);
            }

            app.UseHttpsRedirection();
            app.Use(EventLimiter);
            app.UseMvc();
        }

        private async Task EventLimiter(HttpContext context, Func<Task> next)
        {
            if (context.Request.Headers.ContainsKey("X-Slack-Retry-Num"))
            {
                await context.Response.WriteAsync("OK");
            }
            await next();
        }

        private async Task RequestSigning(HttpContext context, Func<Task> next)
        {
            string slackSignature = context.Request.Headers["x-slack-signature"];
            long timestamp = long.Parse(context.Request.Headers["x-slack-request-timestamp"]);
            string signingSecret = Environment.GetEnvironmentVariable("SLACK_SIGNING_SECRET");
            string content;

            if (slackSignature == null)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("Slack signature missing");
                return;
            }

            context.Request.EnableRewind();

            using (StreamReader reader = new StreamReader(context.Request.Body, Encoding.UTF8, true, 1024, true))
            {
                content = await reader.ReadToEndAsync();
            }

            context.Request.Body.Position = 0;

            string sigBase = string.Format("v0:{0}:{1}", timestamp, content);

            UTF8Encoding encoding = new UTF8Encoding();

            var hmac = new HMACSHA256(encoding.GetBytes(signingSecret));
            byte[] hashBytes = hmac.ComputeHash(encoding.GetBytes(sigBase));

            var ganeratedSignature = "v0=" + BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

            if (ganeratedSignature == slackSignature)
            {
                await next();
            }
            else
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("Request signing failed");
            }
        }
    }
}
