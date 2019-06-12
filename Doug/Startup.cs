using System;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Doug.Commands;
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

            services.AddTransient<IAuthorizationService, AuthorizationService>();

            services.AddTransient<ICoffeeCommands, CoffeeCommands>();
            services.AddTransient<ISlursCommands, SlursCommands>();
            services.AddTransient<ICreditsCommands, CreditsCommands>();
            services.AddTransient<ICoffeeService, CoffeeService>();

            services.AddTransient<IChannelRepository, ChannelRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<ICoffeeRepository, CoffeeRepository>();
            services.AddTransient<ISlurRepository, SlurRepository>();


            services.AddHangfire(config => config.UseSQLiteStorage("Data Source=jobs.db;"));
            services.AddHangfireServer();


            services.AddDbContext<DougContext>(options => options.UseSqlite("Data Source=doug.db"));

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
    }
}
