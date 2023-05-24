using System;
using System.Collections.Generic;
using MeshGraphLib.Core;

namespace MeshGraphLib.Algorithms
{
	public class CatmullClark
	{
		GraphXYZ v_graph;

		GraphXYZ f_graph;

		iFace[] faces;

		XYZ[] face_centers;

		Dictionary<int, XYZ> new_points;

		public CatmullClark(GraphXYZ v_graph, GraphXYZ f_graph, iFace[] faces)
		{
			this.v_graph = v_graph;
			this.f_graph = f_graph;
			this.faces = faces;
			
			ComputeFaceCenters();
		}

		public GraphXYZ Subdivide(int iterations, out iFace[] faces)
		{
			new_points = new Dictionary<int, XYZ>();
		}

		private void ComputeFaceCenters()
		{
			face_centers = new XYZ[faces.Length];


			for (int i = 0; i < faces.Length; i++)
			{
				iFace face = faces[i];

				if (face.IsTriangle) { face_centers[i] = (v_graph.GetNode(face.A) + v_graph.GetNode(face.B) + v_graph.GetNode(face.C)) / 3; }
				else { face_centers[i] = (v_graph.GetNode(face.A) + v_graph.GetNode(face.B) + v_graph.GetNode(face.C) + v_graph.GetNode(face.D)) / 4; }	
			}
        }

		private List<iFace> ComputeEdgePoints(int face_id)
		{
			
			HashSet<int> adjacent_faces = f_graph.GetConnectedNodes(face_id);

			int num_edges_face = faces[face_id].IsTriangle ? 3 : 4;

			Dictionary<int, iEdge> shared_edges = new List<iEdge>();
			List<iEdge> naked_edges = new List<iEdge>();

			foreach (int i in adjacent_faces)
			{
				iEdge shared = FindSharedEdge(faces[face_id],faces[i]);
				shared_edges.Add(Core.Helper.Spatial.ComputeIndexHash(shared.id_a, shared.id_b), shared);
			}

			if(shared_edges.Count != num_edges_face)
			{
				foreach (iEdge  in collection)
				{
					
				}
			}


		}

		private iEdge FindSharedEdge(iFace A, iFace B)
		{
			foreach (iEdge e1 in A.GetEdges())
			{
				foreach (iEdge e2 in B.GetEdges())
				{
					if(e1.IsIdentical(e2)) {return e1;}
				}
			}

			return new iEdge(-1 ,-1);
		}

	}
}

