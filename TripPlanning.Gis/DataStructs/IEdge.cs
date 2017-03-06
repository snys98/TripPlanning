using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripPlanning.Gis.DataStructs
{
    public interface IEdge
    {
        double Weight { get; set; }
        INode Start { get; set; }
        INode End { get; set; }
    }
}
