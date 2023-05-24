using System;
using System.Linq;
using System.Collections.Generic;
using MeshGraphLib.Core;
using MeshGraphLib.Algorithms.Walk.Interfaces;

namespace MeshGraphLib.Algorithms.Walk
{
    public class WalkBFS : IWalk
    {
        private GraphXYZ graph;

        public WalkBFS(GraphXYZ graph)
        {
            this.graph = graph;
        }

        public List<iEdge> Walk(IEnumerable<int> sources)
        {
            Queue<int> to_search = new Queue<int>();
            HashSet<int> visited_nodes = new HashSet<int>(sources);
            List<iEdge> edges = new List<iEdge>();

            foreach (int id in sources) { to_search.Enqueue(id); }

            while (to_search.Count > 0)
            {
                int current = to_search.Dequeue();
                visited_nodes.Add(current);

                foreach (int id in this.graph.GetConnectedNodes(current))
                {
                    if (visited_nodes.Contains(id)) { continue; }

                    visited_nodes.Add(id);
                    edges.Add(new iEdge(current, id));
                    to_search.Enqueue(id);
                }
            }

            return edges;

        }
    }
}

