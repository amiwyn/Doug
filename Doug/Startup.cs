using System;
using System.Net;
using System.Net.Http;
using Doug.Commands;
using Doug.Effects;
using Doug.Items;
using Doug.Middlewares;
using Doug.Repositories;
using Doug.Services;
using Doug.Services.MenuServices;
using Doug.Skills;
using Doug.Slack;
using Hangfire;
using Hangfire.SQLite;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Doug
{
    public class Startup
    {
        private readonly ILogger<Startup> _logger;

        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            _logger = logger;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                };
            });

            services.AddScoped<EventLimiting>();
            services.AddScoped<RequestSigning>();
            services.AddScoped<Authentication>();

            RegisterDougServices(services);

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

        public static IServiceCollection CreateStaticDougContext()
        {
            var services = new ServiceCollection();
            RegisterDougServices(services);

            var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__dougbotdb");
            services.AddDbContext<DougContext>(options => options.UseSqlServer(connectionString));
            return services;
        }

        public static void RegisterDougServices(IServiceCollection services)
        {
            services.AddHttpClient<ISlackWebApi, SlackWebApi>();

            services.AddScoped<IEventDispatcher, EventDispatcher>();
            services.AddScoped<IEffectFactory, EffectFactory>();
            services.AddScoped<ISkillFactory, SkillFactory>();
            services.AddScoped<IEquipmentEffectFactory, EquipmentEffectFactory>();
            services.AddScoped<IActionFactory, ActionFactory>();
            services.AddScoped<ITargetActionFactory, TargetActionFactory>();

            services.AddScoped<IEventService, EventService>();
            services.AddScoped<IAuthorizationService, AuthorizationService>();
            services.AddScoped<ICoffeeService, CoffeeService>();
            services.AddScoped<IRandomService, RandomService>();
            services.AddScoped<IShopMenuService, ShopMenuService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IStatsMenuService, StatsMenuService>();
            services.AddScoped<IInventoryMenuService, InventoryMenuService>();
            services.AddScoped<IShopService, ShopService>();
            services.AddScoped<IGovernmentService, GovernmentService>();
            services.AddScoped<ICombatService, CombatService>();
            services.AddScoped<IMonsterService, MonsterService>();
            services.AddScoped<IMonsterMenuService, MonsterMenuService>();
            services.AddScoped<IPartyMenuService, PartyMenuService>();
            services.AddScoped<IPartyService, PartyService>();
            services.AddScoped<ICraftingMenuService, CraftingMenuService>();
            services.AddScoped<ICraftingService, CraftingService>();
            services.AddScoped<ISkillService, SkillService>();
            services.AddScoped<IInventoryService, InventoryService>();
            services.AddScoped<ILuaService, LuaService>();

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
            services.AddScoped<IEffectRepository, EffectRepository>();
            services.AddScoped<IGovernmentRepository, GovernmentRepository>();
            services.AddScoped<IShopRepository, ShopRepository>();
            services.AddScoped<ISpawnedMonsterRepository, SpawnedMonsterRepository>();
            services.AddScoped<ICreditsRepository, CreditsRepository>();
            services.AddScoped<IPartyRepository, PartyRepository>();
            services.AddScoped<IRecipeRepository, RecipeRepository>();
            services.AddScoped<IItemRepository, ItemRepository>();
            services.AddScoped<IMonsterRepository, MonsterRepository>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCors(builder =>
                builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseExceptionHandler(DougExceptionHandler);
            app.UseHttpsRedirection();
            app.UseMiddleware<EventLimiting>();
            app.UseRouting();

            app.UseWhen(context => context.Request.Path.StartsWithSegments("/cmd"), appBuilder => appBuilder.UseMiddleware<RequestSigning>());
            app.UseWhen(context => context.Request.Path.StartsWithSegments("/ui"), appBuilder => appBuilder.UseMiddleware<Authentication>());

            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }

        private void DougExceptionHandler(IApplicationBuilder builder)
        {
            builder.Run(async context =>
            {
                context.Response.StatusCode = (int)HttpStatusCode.OK;
                var error = context.Features.Get<IExceptionHandlerFeature>();
                if (error != null)
                {
                    _logger.LogError(error.Error, context.Request.Path.ToString());
                    await context.Response.WriteAsync(string.Format(DougMessages.DougError, error.Error.Message)).ConfigureAwait(false);
                }
            });
        }
    }
}
