using System;
using System.Collections.Generic;
using Rhino.Geometry;

namespace QuadGraphLib.Core
{
	public class UndirectedGraph
	{
		private List<XYZ> nodes_xyz {get; set;} 

		private Dictionary<int, int> nodes_map {get; set;} 
		
		private Dictionary<int, List<int>> nodes_conn {get; set;}

		public UndirectedGraph()
		{
			nodes_xyz = new List<XYZ>();
			nodes_map = new Dictionary<int, int>();
			nodes_conn = new Dictionary<int, List<int>>();
		}

		public bool HasNode(XYZ node) => nodes_map.ContainsKey(node.SpatialHash);

		public List<XYZ> TryAddNodes(IEnumerable<XYZ> nodes)
		{
			List<XYZ> failed = new List<XYZ>();

			foreach (XYZ node in nodes)
			{
				if(!TryAddNode(node))
				{
					failed.Add(node);
				}
			}

			return failed;
		}

		public bool TryAddNode(XYZ node)
		{
			if(nodes_map.ContainsKey(node.SpatialHash))
			{
				return false;
			}

			nodes_map.Add(node.SpatialHash, nodes_xyz.Count);
			nodes_conn.Add(node.SpatialHash, new List<int>());
			nodes_xyz.Add(node);
			
			return true;
		}

		public bool TryAddEdge(EdgeXYZ edge, bool add_nodes = false)
		{
			if(add_nodes)
			{
				TryAddNode(edge.A);
				TryAddNode(edge.B);
			}
			else if(!HasNode(edge.A) || !HasNode(edge.B))
			{
				return false;
			}

			nodes_conn[nodes_map[edge.A.SpatialHash]].Add(nodes_map[edge.B.SpatialHash]);

			return true;
		}

		public List<EdgeXYZ> GetEdges()
		{
			List<EdgeXYZ> edges = new List<EdgeXYZ>();

			HashSet<int> visited_nodes = new HashSet<int>();
 
			foreach (var pair in nodes_conn)
			{
				XYZ base_node = nodes_xyz[pair.Key];
				visited_nodes.Add(pair.Key);

				for (int i = 0; i < pair.Value.Count; i++)
				{
					if(visited_nodes.Contains(pair.Value[i])
					{
						continue;
					}
					
					edges.Add()
				}
			}
		}
	}
}

