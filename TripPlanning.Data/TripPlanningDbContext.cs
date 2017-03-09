using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TripPlanning.Data.Entities;

namespace TripPlanning.Data
{
    class TripPlanningDbContext:DbContext
    {
        public DbSet<Airport> Airports { get; set; }
        public DbSet<TrainStation> TrainStations { get; set; }
        public DbSet<FlightSegment> FlightSegments { get; set; }
        public DbSet<TrainSegment> TrainSegments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}
