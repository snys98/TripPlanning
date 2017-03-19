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
        public string FlightNumber { get; set; }
        public int TimeCost { get; set; }
        public int Price { get; set; }
        [ForeignKey(nameof(DeptAirportName))]
        public AirStation DeptAirport { get; set; }
        public string DeptAirportName { get; set; }
        [ForeignKey(nameof(DestAirportName))]
        public AirStation DestAirport { get; set; }
        public string DestAirportName { get; set; }
        public string DeptTime { get; set; }
        public string ArriTime { get; set; }
        public int PlusDays { get; set; }
    }
}
