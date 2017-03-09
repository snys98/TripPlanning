using System.Collections.Generic;
using TripPlanning.Gis.DataStructs;

namespace TripPlanning.Data.Entities
{
    public class Route:IPath
    {
        public INode StartNode { get; set; }
        public INode EndNode { get; set; }
        public List<INode> Nodes { get; set; }
        public double TotalWeight { get; set; }
    }
}
