using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripPlanning.Gis.DataStructs;

namespace TripPlanning.Lib.Entities
{
    public class Route:IPath
    {
        public INode StartNode { get; set; }
        public INode EndNode { get; set; }
        public List<INode> Nodes { get; set; }
        public double TotalWeight { get; set; }
    }
}
