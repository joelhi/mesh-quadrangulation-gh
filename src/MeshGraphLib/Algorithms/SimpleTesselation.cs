using System;
using System.Collections.Generic;
using MeshGraphLib.Core;

namespace MeshGraphLib.Algorithms
{
	public class SimpleTesselation
	{

		GraphXYZ v_graph;

		iFace[] faces;

		public SimpleTesselation(GraphXYZ v_graph, iFace[] faces)
		{
			this.v_graph = v_graph;
			this.faces = faces;
		}

		public GraphXYZ Subdivide(int iterations, out iFace[] out_faces)
		{
			for (int i = 0; i < faces.Length; i++)
			{
				iFace face = faces[i];
				iEdge[] edges = face.GetEdges();

				XYZ face_center = ComputeFaceCenter(i);

				XYZ[] edge_center = ComputeEdgeCenters(edges);

			}
		}

		private XYZ ComputeFaceCenter(int i)
		{
				iFace face = faces[i];

				if (face.IsTriangle) { return (v_graph.GetNode(face.A) + v_graph.GetNode(face.B) + v_graph.GetNode(face.C)) / 3; }
				else { return (v_graph.GetNode(face.A) + v_graph.GetNode(face.B) + v_graph.GetNode(face.C) + v_graph.GetNode(face.D)) / 4; }	
        }

		private XYZ[] ComputeEdgeCenters(iEdge[] edges)
		{
			XYZ[] edge_center = new XYZ[edges.Length];

			for (int i = 0; i < edges.Length; i++)
			{
				edge_center[i] = (v_graph.GetNode(edges[i].id_a) + v_graph.GetNode(edges[i].id_b)) * 0.5;
			}

			return edge_center;
		}
	}
}

