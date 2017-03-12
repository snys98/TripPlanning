namespace TripPlanning.GisAnalysis.DataStructs
{
    public struct Edge
    {
        public double Weight { get; set; }
        public Node Start { get; set; }
        public Node End { get; set; }
    }
}
