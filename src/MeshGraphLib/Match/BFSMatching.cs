using System;
using System.Collections.Generic;
using System.Linq;
using MeshGraphLib.Core;
using MeshGraphLib.Match.Selection;

namespace MeshGraphLib.Match
{
	public class BFSMatching
	{
	    private GraphXYZ graph;

	    private IMatchSelection selection_criteria;

	    public BFSMatching(GraphXYZ graph, IMatchSelection selection_criteria)
	    {
		this.graph = graph;
		this.selection_criteria = selection_criteria;
	    }

	    public List<iEdge> ComputeMatchings(IEnumerable<int> sources, out List<int> singles)
	    {
            	Queue<int> to_search = new Queue<int>();
            	HashSet<int> visited_nodes = new HashSet<int>();
            	List<iEdge> edges = new List<iEdge>();
            	singles = new List<int>();

            	foreach (int id in sources) { to_search.Enqueue(id); }

            	while (to_search.Count > 0)
            	{
                    int current = to_search.Dequeue();
                    if (!visited_nodes.Add(current)) { continue; }

                    List<iEdge> node_edges = new List<iEdge>();

                    foreach (int id in this.graph.GetConnectedNodes(current))
                    {
                        if (visited_nodes.Contains(id)) { continue; }

                        node_edges.Add(new iEdge(current, id));
                    }

                    if (node_edges.Count == 0)
                    { 
                        singles.Add(current);
                        continue;
                    }

                    iEdge selected = selection_criteria.PickMatching(node_edges, graph, out int[] remaining);

                    visited_nodes.Add(selected.id_b);
                    edges.Add(selected);

                    for (int i = 0; i < remaining.Length; i++) { to_search.Enqueue(remaining[i]); }
                }


                return edges;
            }
	}
}

