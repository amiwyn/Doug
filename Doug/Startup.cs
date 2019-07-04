using System;
using System.IO;
using System.Net;
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
using Microsoft.AspNetCore.Diagnostics;
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

            
            services.AddScoped<ISlackWebApi, SlackWebApi>();
            services.AddScoped<IItemEventDispatcher, ItemEventDispatcher>();
            services.AddScoped<IItemFactory, ItemFactory>();

            services.AddScoped<IEventService, EventService>();
            services.AddScoped<IAuthorizationService, AuthorizationService>();
            services.AddScoped<ICoffeeService, CoffeeService>();
            services.AddScoped<IRandomService, RandomService>();
            services.AddScoped<IShopMenuService, ShopMenuService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IStatsMenuService, StatsMenuService>();
            services.AddScoped<IInventoryMenuService, InventoryMenuService>();

            services.AddScoped<ICoffeeCommands, CoffeeCommands>();
            services.AddScoped<ISlursCommands, SlursCommands>();
            services.AddScoped<ICreditsCommands, CreditsCommands>();
            services.AddScoped<ICasinoCommands, CasinoCommands>();
            services.AddScoped<ICombatCommands, CombatCommands>();
            services.AddScoped<IInventoryCommands, InventoryCommands>();
            services.AddScoped<IStatsCommands, StatsCommands>();

            services.AddScoped<IChannelRepository, ChannelRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICoffeeRepository, CoffeeRepository>();
            services.AddScoped<ISlurRepository, SlurRepository>();
            services.AddScoped<IStatsRepository, StatsRepository>();
            services.AddScoped<IInventoryRepository, InventoryRepository>();
            services.AddScoped<IEquipmentRepository, EquipmentRepository>();

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

            app.UseExceptionHandler(DougExceptionHandler);

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
            var timestamp = long.Parse(context.Request.Headers["x-slack-request-timestamp"]);
            var signingSecret = Environment.GetEnvironmentVariable("SLACK_SIGNING_SECRET");
            string content;

            if (slackSignature == null)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("Slack signature missing");
                return;
            }

            context.Request.EnableRewind();

            using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, true, 1024, true))
            {
                content = await reader.ReadToEndAsync();
            }

            context.Request.Body.Position = 0;

            var sigBase = $"v0:{timestamp}:{content}";

            var encoding = new UTF8Encoding();

            var hmac = new HMACSHA256(encoding.GetBytes(signingSecret));
            var hashBytes = hmac.ComputeHash(encoding.GetBytes(sigBase));

            var generatedSignature = "v0=" + BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

            if (generatedSignature == slackSignature)
            {
                await next();
            }
            else
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("Request signing failed");
            }
        }

        private void DougExceptionHandler(IApplicationBuilder builder)
        {
            builder.Run(async context =>
            {
                context.Response.StatusCode = (int)HttpStatusCode.OK;
                var error = context.Features.Get<IExceptionHandlerFeature>();
                if (error != null)
                {
                    await context.Response.WriteAsync(string.Format(DougMessages.DougError, error.Error.Message)).ConfigureAwait(false);
                }
            });
        }
    }
}
