using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace TripPlanning.Data.Entities
{
    public class Airport:IStation
    {
        [Key]
        public string AirportName { get; set; }
        public string CityCode { get; set; }
        [ForeignKey(nameof(CityCode))]
        public City City { get; set; }

        string IStation.StationName {
            get => this.AirportName;
            set => this.AirportName = value;
        }
    }
}
