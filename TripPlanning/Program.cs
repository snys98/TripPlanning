using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using TripPlanning.Data;
using TripPlanning.Data.Entities;

namespace TripPlanning
{
    public class Program
    {
        public static IConfigurationRoot Configuration { get; } = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).Build();
        public static IServiceCollection Services { get; } = ConfigureServices(new ServiceCollection());

        static void Main(string[] args)
        {
            Console.WriteLine(Configuration.GetSection("DefaultConnection"));

            #region InitTrainStation

            #endregion

            #region InitAirport

            #endregion

            #region InitTrains

            #endregion

            #region InitFlights

            #endregion
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public static IServiceCollection ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<TripPlanningDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
                options.UseMemoryCache(new MemoryCache(new MemoryCacheOptions() { ExpirationScanFrequency = TimeSpan.FromMinutes(30) }));
            });

            // Add framework services.

            services.AddLogging();

            // #region account related services

            // services.AddSingleton<IEmailSender>(AuthMessageSender.Instance)
            //     .AddSingleton<ISmsSender>(AuthMessageSender.Instance);

            // #endregion

            //services.AddViewModels();
            return services;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(ILoggerFactory loggerFactory,
            TripPlanningDbContext dbContext)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
        }
    }
}