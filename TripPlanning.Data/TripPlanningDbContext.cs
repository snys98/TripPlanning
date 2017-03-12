using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TripPlanning.Data.Entities;
using TripPlanning.GisAnalysis.DataStructs;

namespace TripPlanning.Data
{
    public class TripPlanningDbContext:DbContext
    {
        public DbSet<Airport> Airports { get; set; }
        public DbSet<TrainStation> TrainStations { get; set; }
        public DbSet<FlightSegment> FlightSegments { get; set; }
        public DbSet<TrainSegment> TrainSegments { get; set; }
        public DbSet<City> Cities { get; set; }

        public TripPlanningDbContext(DbContextOptions options)
            :base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<City>().HasBaseType<Node>();
            //modelBuilder.Entity<FlightSegment>().HasOne(x => );
            //modelBuilder.Entity<FlightSegment>().HasOne(x => (City) x.End);
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}
