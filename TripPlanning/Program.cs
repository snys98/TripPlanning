using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using TripPlanning.Data;
using TripPlanning.Data.Entities;
using TripPlanning.GisAnalysis;
using TripPlanning.GisAnalysis.Extensions;

namespace TripPlanning
{
    public class Program
    {
        static Program()
        {
            Configuration = new ConfigurationBuilder().SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appconfig.json").Build();
            Services = ConfigureServices(new ServiceCollection());
            ServiceProvider = new DefaultServiceProviderFactory().CreateServiceProvider(Services);
            ServiceProvider.GetRequiredService<ILoggerFactory>()
                .AddConsole(Configuration.GetSection("Logging"))
                .AddDebug();

            DbContext = ServiceProvider.GetRequiredService<TripPlanningDbContext>();
        }

        private static TripPlanningDbContext DbContext { get; set; }

        private static IConfigurationRoot Configuration { get; }

        private static IServiceCollection Services { get; }

        private static IServiceProvider ServiceProvider { get; }

        static void Main(string[] args)
        {
            switch (args[0])
            {
                case "i":
                    Task[] tasks = {};
                    

                    if (DbContext.Database.EnsureCreated())
                    {
                        tasks[0] = new Task(InitCityData);
                        tasks[1] = new Task(InitAirportData);
                        tasks[2] = new Task(InitTraiStationData);
                        tasks[3] = new Task(InitFlightSegmentData);
                        tasks[4] = new Task(InitCitySegmentData);
                    }
                    Debug.WriteLine("wait task to be finished");
                    while (tasks.Any(x=>!x.IsCompleted))
                    {
                        Debug.Write('.');
                        Thread.Sleep(1000);
                        if (tasks.Any(x=>x.IsFaulted))
                        {
                            Debug.WriteLine("failed");
                            break;
                        }
                    }
                    
                    Console.ReadKey();
                    break;
                case "":
                    var resolver = ServiceProvider.GetRequiredService<IPathResolver>();
                    break;
            }
            Console.ReadKey();
        }

        private static void InitCitySegmentData()
        {
            using (StreamReader reader = File.OpenText(Path.Combine(AppContext.BaseDirectory, "Data", "station.txt")))
            {
                while (!reader.EndOfStream)
                {
                    //北京北: 北京: 116.353817,39.942789
                    var line = reader.ReadLine();
                    List<string> splitLine = line.Split(':', ' ', ',').Skip(1).ToList();
                    if (splitLine.Count == 3)
                    {
                        var entry = new City()
                        {
                            CityName = splitLine[0],
                            X = double.Parse(splitLine[1]),
                            Y = double.Parse(splitLine[2])
                        };
                        DbContext.Cities.Add(entry);
                    }
                }
                DbContext.SaveChanges();
            }
        }

        private static void InitFlightSegmentData()
        {
            using (StreamReader reader = File.OpenText(Path.Combine(AppContext.BaseDirectory, "Data", "station.txt")))
            {
                while (!reader.EndOfStream)
                {
                    //北京北: 北京: 116.353817,39.942789
                    var line = reader.ReadLine();
                    List<string> splitLine = line.Split(':', ' ', ',').ToList();
                    if (splitLine.Count == 4)
                    {
                        var entry = new City()
                        {
                            CityName = splitLine[1],
                            X = double.Parse(splitLine[2]),
                            Y = double.Parse(splitLine[3])
                        };
                        DbContext.Cities.Add(entry);
                    }
                }
                DbContext.SaveChanges();
            }
        }

        private static void InitTraiStationData()
        {
            using (StreamReader reader = File.OpenText(Path.Combine(AppContext.BaseDirectory, "Data", "airport.txt")))
            {
                while (!reader.EndOfStream)
                {
                    //南苑机场,116.397174,39.791233
                    var line = reader.ReadLine();
                    List<string> splitLine = line.Split(':', ' ', ',').Skip(1).ToList();
                    if (splitLine.Count == 3)
                    {
                        var entry = new TrainStation()
                        {
                            CityCode = splitLine[1],
                            StationName = splitLine[0],
                        };
                        DbContext.TrainStations.Add(entry);
                    }
                }
                DbContext.SaveChanges();
            }
        }

        private static void InitAirportData()
        {
            using (StreamReader reader = File.OpenText(Path.Combine(AppContext.BaseDirectory, "Data", "station.txt")))
            {
                while (!reader.EndOfStream)
                {
                    //北京北: 北京: 116.353817,39.942789
                    var line = reader.ReadLine();
                    List<string> splitLine = line.Split(':', ' ', ',').Skip(1).ToList();
                    if (splitLine.Count == 3)
                    {
                        var entry = new City()
                        {
                            CityName = splitLine[0],
                            X = double.Parse(splitLine[1]),
                            Y = double.Parse(splitLine[2])
                        };
                        DbContext.Cities.Add(entry);
                    }
                }
                DbContext.SaveChanges();
            }
        }

        private static void InitCityData()
        {
            using (StreamReader reader = File.OpenText(Path.Combine(AppContext.BaseDirectory, "Data", "station.txt")))
            {
                while (!reader.EndOfStream)
                {
                    //北京北: 北京: 116.353817,39.942789
                    var line = reader.ReadLine();
                    List<string> splitLine = line.Split(':', ' ', ',').Skip(1).ToList();
                    if (splitLine.Count == 3)
                    {
                        var entry = new City()
                        {
                            CityName = splitLine[0],
                            X = double.Parse(splitLine[1]),
                            Y = double.Parse(splitLine[2])
                        };
                        DbContext.Cities.Add(entry);
                    }
                }
                DbContext.SaveChanges();
            }
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public static IServiceCollection ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<TripPlanningDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("TripPlanningDatabase"));
                options.UseMemoryCache(new MemoryCache(new MemoryCacheOptions() {ExpirationScanFrequency = TimeSpan.FromMinutes(30)}));
            });

            services.AddLogging();

            services.AddPathResolver(Configuration.GetSection("GeoAnalysis"));

            return services;
        }
    }
}