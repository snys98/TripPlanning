using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripPlanning.Gis.DataStructs
{
    public interface IPath
    {
        INode StartNode { get; set; }

        INode EndNode { get; set; }
        List<INode> Nodes { get; set; }

        double TotalWeight { get; set; }
    }
}
