using System;
using System.Linq;
using System.Collections.Generic;
using MeshGraphLib.Core;
using MeshGraphLib.Walk.Interfaces;

namespace MeshGraphLib.Walk
{
	public class WalkBFS : IWalk
	{
		private GraphXYZ graph;

		public WalkBFS(GraphXYZ graph)
		{
			this.graph = graph;
		}

        public GraphXYZ Walk(IEnumerable<int> start_indices)
        {
            Queue<int> to_search = new Queue<int>();
			HashSet<int> visited_nodes = new HashSet<int>(start_indices);
			List<iEdge> edges = new List<iEdge>();

			foreach (int id in start_indices){ to_search.Enqueue(id); }

			while (to_search.Count > 0)
			{
				int current = to_search.Dequeue();
				visited_nodes.Add(current);

				foreach (int id in this.graph.GetConnectedNodes(current))
				{
					if(visited_nodes.Contains(id)){ continue;}

					visited_nodes.Add(id);
					edges.Add(new iEdge(current, id));
					to_search.Enqueue(id);
				}
			}

			// Construct new graph
			GraphXYZ bfs_graph = new GraphXYZ();

			bfs_graph.TryAddNodes(graph.GetNodes());

			for (int i = 0; i < edges.Count; i++)
			{
				bfs_graph.TryAddEdge(edges[i].id_a, edges[i].id_b);
			}

			return bfs_graph;

        }
    }
}

