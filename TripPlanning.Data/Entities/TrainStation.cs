using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TripPlanning.Data.Entities
{
    public class TrainStation
    {
        [Key]
        public string StationName { get; set; }
        public string CityCode { get; set; }
        [ForeignKey(nameof(CityCode))]
        public City City { get; set; }
    }
}
