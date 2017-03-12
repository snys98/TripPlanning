using System.Collections.Generic;

namespace TripPlanning.GisAnalysis.DataStructs
{
    public class Path
    {
        public Node StartNode { get; set; }
        public Node EndNode { get; set; }
        public List<Node> Nodes { get; set; }
        public double TotalWeight { get; set; }
    }
}
