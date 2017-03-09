using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripPlanning.Gis.DataStructs;

namespace TripPlanning.Data.Entities
{
    public class TrainSegment:IEdge
    {
        public double Weight { get; set; }
        public INode Start { get; set; }
        public INode End { get; set; }
    }
}
