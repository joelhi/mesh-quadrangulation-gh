using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel.Geometry.Delaunay;
using Grasshopper.Kernel.Graphs;
using MeshGraphLib.Core;
using MeshGraphLib.Core.Helper;

namespace MeshGraphLib.Match.Selection
{
	public class EdgeLengthSelection : IMatchSelection
	{
		public EdgeLengthSelection()
		{
		}

        public iEdge PickMatching(IEnumerable<iEdge> edges, GraphXYZ graph, out int[] remaining_nodes)
        {
            double minDistance = double.MaxValue;
            iEdge selected_edge = new iEdge(-1, -1);

            foreach(iEdge e in edges)
            {
                double len = graph.GetNode(e.id_a).DistanceTo(graph.GetNode(e.id_b));

                if (len < minDistance)
                {
                    selected_edge = e;
                    minDistance = len;
                }
            }

            HashSet<int> remaining = graph.GetConnectedNodes(selected_edge.id_b);

            edges.Select(e => remaining.Add(e.id_b));
            remaining.Remove(selected_edge.id_b);

            remaining_nodes = remaining.ToArray();

            return selected_edge;
        }
    }
}

