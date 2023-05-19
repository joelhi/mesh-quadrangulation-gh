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
			if(nodes_map.ContainsKey(node.SpatialHash))
			{
				return false;
			}

			nodes_map.Add(node.SpatialHash, nodes_xyz.Count);
			nodes_xyz.Add(node);
			
			return true;
		}

		public bool TryAddEdge(EdgeXYZ edge)
		{
			if(edge.A.SpatialHash)
		}
	}
}

