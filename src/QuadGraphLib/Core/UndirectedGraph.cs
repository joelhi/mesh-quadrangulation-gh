using System;
using System.Collections.Generic;
using Rhino.Geometry;

namespace QuadGraphLib.Core
{
	public class UndirectedGraphXYZ
	{
		private List<XYZ> nodes_xyz {get; set;} 

		private Dictionary<int, int> nodes_map {get; set;} 
		
		private List<HashSet<int>> nodes_conn {get; set;}

		public UndirectedGraphXYZ()
		{
			nodes_xyz = new List<XYZ>();
			nodes_map = new Dictionary<int, int>();
			nodes_conn = new List<HashSet<int>>();
		}

		public bool HasNode(XYZ node) => nodes_map.ContainsKey(node.SpatialHash);

		public int NodeIndex(XYZ node) => nodes_map[node.SpatialHash];

		public bool HasEdge(EdgeXYZ edge)
		{
			if(nodes_conn[NodeIndex(edge.A)].Contains(NodeIndex(edge.B)))
			{
				return true;
			}

			return nodes_conn[NodeIndex(edge.B)].Contains(NodeIndex(edge.A));
		}

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
			if(HasNode(node))
			{
				return false;
			}

			nodes_map.Add(node.SpatialHash, nodes_xyz.Count);
			nodes_xyz.Add(node);
			nodes_conn.Add(new HashSet<int>());
			
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

			if(HasEdge(edge))
			{
				return false;
			}

			nodes_conn[nodes_map[edge.A.SpatialHash]].Add(nodes_map[edge.B.SpatialHash]);
			nodes_conn[nodes_map[edge.B.SpatialHash]].Add(nodes_map[edge.A.SpatialHash]);

			return true;
		}

		public List<EdgeXYZ> GetEdges()
		{
			List<EdgeXYZ> edges = new List<EdgeXYZ>();

			HashSet<int> visited_nodes = new HashSet<int>();
			
			for (int i = 0; i < nodes_xyz.Count; i++)
			{
				visited_nodes.Add(i);

				foreach (int id in nodes_conn[i])
				{
					if(visited_nodes.Contains(id))
					{
						continue;
					}

					edges.Add(new EdgeXYZ(nodes_xyz[i], nodes_xyz[id]));
				}
			}

			return edges;
		}
	}
}

