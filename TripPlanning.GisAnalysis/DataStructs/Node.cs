using System;
using System.Collections.Generic;

namespace TripPlanning.GisAnalysis.DataStructs
{
    ///结点,用于模拟城市
    public struct Node
    {
        public List<SubNode> SubNodes { get; set; }
        public double X { get; set; }
        public double Y { get; set; }

        public static bool operator == (Node node1, Node node2)
        {
            return Math.Abs(node1.X - node2.X) < GeoAnalysisConfig.Tolerance && Math.Abs(node2.Y - node2.Y) < GeoAnalysisConfig.Tolerance;
        }

        public Node(double x, double y)
        {
            SubNodes = new List<SubNode>();
            this.X = x;
            this.Y = y;
        }


        public static bool operator !=(Node node1, Node node2)
        {
            return !(node1 == node2);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

}
