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
    public class TrainSegment
    {
        [Key]
        public int Id { get; set; }
        public string TrainNumber { get; set; }
        public int TimeCost { get; set; }
        public int Price { get; set; }
        [ForeignKey(nameof(DeptStationName))]
        public TrainStation DeptStation { get; set; }
        public string DeptStationName { get; set; }
        [ForeignKey(nameof(DestStationName))]
        public TrainStation DestStation { get; set; }
        public string DestStationName { get; set; }
    }
}
