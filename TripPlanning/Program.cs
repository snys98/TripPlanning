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
                        tasks[0] = new Task(InitCityAndTrainStationData).ContinueWith((x)=> {
                            InitTrainSegmentData();
                        });
                        tasks[0].Start();
                        tasks[1] = new Task(InitAirportData).ContinueWith((x) =>
                        {
                            InitFlightSegmentData();
                        });
                        tasks[1].Start();
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

        private static void InitCityAndTrainStationData()
        {
            using (StreamReader reader = File.OpenText(Path.Combine(AppContext.BaseDirectory, "Data", "station.txt")))
            {
                while (!reader.EndOfStream)
                {
                    //北京北: 北京: 116.353817,39.942789
                    var line = reader.ReadLine();
                    List<string> splitLine = line.Split(new char[] { ':', ' ', ',' },StringSplitOptions.RemoveEmptyEntries).ToList();
                    if (splitLine.Count == 4)
                    {
                        var city = new City()
                        {
                            CityName = splitLine[1],
                        };
                        DbContext.Cities.Add(city);
                        var station = new TrainStation()
                        {
                            CityName = splitLine[1],
                            StationName = splitLine[0],
                            X = double.Parse(splitLine[2]),
                            Y = double.Parse(splitLine[3])
                        };
                        DbContext.TrainStations.Add(station);
                    }
                }
                DbContext.SaveChanges();
            }
        }
        private static void InitAirportData()
        {
            using (StreamReader reader = File.OpenText(Path.Combine(AppContext.BaseDirectory, "Data", "flight.txt")))
            {
                HashSet<string> coveredAirports = new HashSet<string>();
                while (!reader.EndOfStream)
                {
                    //KN5987,南苑机场,浦东国际机场,21:55,00:15,370,北京,上海
                    var line = reader.ReadLine();
                    List<string> splitLine = line.Split(' ', ',').ToList();
                    if (splitLine.Count == 8)
                    {
                        if (coveredAirports.Add(splitLine[1]))
                        {
                            var entry = new AirStation()
                            {
                                CityName = splitLine[6],
                                StationName = splitLine[1],
                            };
                            DbContext.Airports.Add(entry);
                        }
                        if (coveredAirports.Add(splitLine[2]))
                        {
                            var entry = new AirStation()
                            {
                                CityName = splitLine[7],
                                StationName = splitLine[2],
                            };
                            DbContext.Airports.Add(entry);
                        }

                    }
                }
                DbContext.SaveChanges();
            }
            using (StreamReader reader = File.OpenText(Path.Combine(AppContext.BaseDirectory, "Data", "airport.txt")))
            {
                while (!reader.EndOfStream)
                {
                    //南苑机场,116.397174,39.791233
                    var line = reader.ReadLine();
                    List<string> splitLine = line.Split(':', ' ', ',').ToList();
                    if (splitLine.Count == 4)
                    {
                        var airport = DbContext.Airports.First(x => x.StationName == splitLine[0]);
                        airport.X = double.Parse(splitLine[1]);
                        airport.Y = double.Parse(splitLine[2]);
                    }
                }
                DbContext.SaveChanges();
            }
        }
        private static void InitFlightSegmentData()
        {
            using (StreamReader reader = File.OpenText(Path.Combine(AppContext.BaseDirectory, "Data", "flight.txt")))
            {
                var airports = DbContext.Airports.Select(x=>x.StationName).ToList();
                while (!reader.EndOfStream)
                {
                    //KN5987,南苑机场,浦东国际机场,21:55,00:15,370,北京,上海
                    var line = reader.ReadLine();
                    List<string> splitLine = line.Split(' ', ',').ToList();
                    if (splitLine.Count == 8)
                    {
                        if (airports.Contains(splitLine[1]) && airports.Contains(splitLine[2]))
                        {
                            int timeCost = CaculateTimeCost(splitLine[3], splitLine[4]);
                            var entry = new FlightSegment() {
                                DeptAirportName = splitLine[1],
                                DeptTime = splitLine[3],
                                DestAirportName = splitLine[2],
                                FlightNumber = splitLine[0],
                                Price = int.Parse(splitLine[5]),
                                TimeCost = timeCost,
                                ArriTime = splitLine[4],
                                PlusDays = (int.Parse(splitLine[3]) + timeCost) % 2400
                            };
                            DbContext.FlightSegments.Add(entry);
                        }
                    }
                }
                DbContext.SaveChanges();
            }
        }

        private static void InitTrainSegmentData()
        {
            var stations = DbContext.TrainStations.Select(x=>x.StationName).ToList();
            using (StreamReader reader = File.OpenText(Path.Combine(AppContext.BaseDirectory, "Data", "train.txt")))
            {
                while (!reader.EndOfStream)
                {
                    //T3017	240	锦州	5	12:17	12:23	6
                    var line = reader.ReadLine();
                    List<string> splitLine = line.Split(' ', '\t', ',').ToList();
                    if (!stations.Contains(splitLine[2]))
                    {
                        continue;
                    }
                    if (splitLine.Count == 7)
                    {
                        int timeCost = CaculateTimeCost(splitLine[3], splitLine[4]);
                        var entry = new FlightSegment()
                        {
                            DeptAirportName = splitLine[1],
                            DeptTime = splitLine[3],
                            DestAirportName = splitLine[2],
                            FlightNumber = splitLine[0],
                            Price = int.Parse(splitLine[5]),
                            TimeCost = timeCost,
                            ArriTime = splitLine[4],
                            PlusDays = (int.Parse(splitLine[3]) + timeCost) % 2400
                        };
                        DbContext.FlightSegments.Add(entry);
                    }
                }
                DbContext.SaveChanges();
            }
        }
        
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

        private static int CaculateTimeCost(string v1, string v2)
        {
            return (int)(TimeSpan.Parse(v2) - TimeSpan.Parse(v1)).TotalMinutes;
        }
    }
}