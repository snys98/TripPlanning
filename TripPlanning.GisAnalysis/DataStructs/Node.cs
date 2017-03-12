using System;

namespace TripPlanning.GisAnalysis.DataStructs
{
    public struct Node
    {
        public double X { get; set; }
        public double Y { get; set; }

        public static bool operator == (Node node1, Node node2)
        {
            return Math.Abs(node1.X - node2.X) < Tolerance && Math.Abs(node2.Y - node2.Y) < Tolerance;
        }

        public Node(double x, double y)
        {
            if (Tolerance <= default(double))
            {
                throw new InvalidOperationException("You must set the tolerance first.");
            }
            this.X = x;
            this.Y = y;
        }

        private static double Tolerance { get; }

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
