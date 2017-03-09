using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.FileProviders;

namespace TripPlanning
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "appconfig.json");
            var jsonConfigSource = new JsonConfigurationSource()
            {
                Path = path,
                ReloadOnChange = true
            };
            var builder = new ConfigurationBuilder();
            builder.Add(jsonConfigSource);
            var item = builder.Build().GetSection("section1");
            Console.WriteLine(item);
            Console.ReadKey();
        }
    }
}