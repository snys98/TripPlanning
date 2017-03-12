using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripPlanning.GisAnalysis.DataStructs;

namespace TripPlanning.Data.Entities
{
    public class FlightSegment
    {
        [Key]
        public int Id { get; set; }
        public double Weight { get; set; }
        [ForeignKey(nameof(DeptAirportCode))]
        public Airport DeptAirport { get; set; }
        public string DeptAirportCode { get; set; }
        [ForeignKey(nameof(DestAirportCode))]
        public Airport DestAirport { get; set; }
        public string DestAirportCode { get; set; }
    }
}
