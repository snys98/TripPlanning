﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TripPlanning.Data.Entities;
using TripPlanning.GisAnalysis.DataStructs;

namespace TripPlanning.Data
{
    public class TripPlanningDbContext:DbContext
    {
        public DbSet<AirStation> Airports { get; set; }
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
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}
