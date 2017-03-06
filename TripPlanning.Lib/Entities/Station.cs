using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripPlanning.Gis.DataStructs;

namespace TripPlanning.Lib.Entities
{
    public class Station:INode
    {
        public double X { get; set; }
        public double Y { get; set; }
    }
}
