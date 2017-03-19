using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TripPlanning.Data.Entities
{
    public class TrainStation
    {
        [Key]
        public string StationName { get; set; }
        public string CityName { get; set; }
        [ForeignKey(nameof(CityName))]
        public City City { get; set; }
        public double X { get ; set ; }
        public double Y { get ; set ; }
    }
}
