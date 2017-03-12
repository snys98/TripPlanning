using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TripPlanning.GisAnalysis.DataStructs;

namespace TripPlanning.Data.Entities
{
    public class City
    {
        public double X { get; set; }
        public double Y { get; set; }
        [Key]
        public string CityCode { get; set; }
        public string CityName { get; set; }
    }
}
