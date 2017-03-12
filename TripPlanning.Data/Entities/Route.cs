using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TripPlanning.GisAnalysis.DataStructs;

namespace TripPlanning.Data.Entities
{
    public class Route
    {
        [Key]
        public int Id { get; set; }
        public City StartCity { get; set; }
        public City EndCity { get; set; }
        public List<City> TransitCities { get; set; }
        public double TotalWeight { get; set; }
    }
}
