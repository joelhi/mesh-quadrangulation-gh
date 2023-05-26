using System;
using System.Collections.Generic;
using System.Linq;
using MeshGraphLib.Core;
using MeshGraphLib.Core.Helper;

namespace MeshGraphLib.Algorithms
{
	public class CatmullClark
	{
		GraphXYZ v_graph;

		GraphXYZ f_graph;

		GraphXYZ subdivided_v_graph;

		iFace[] faces;

		XYZ[] face_centers;

		Dictionary<int, HashSet<int>> edge_faces;

		Dictionary<int, iEdge> edge_map;

		Dictionary<int, XYZ> edge_points;

		List<List<XYZ>> new_Points;

		public CatmullClark(GraphXYZ v_graph, GraphXYZ f_graph, iFace[] faces)
		{
			this.v_graph = v_graph;
			this.f_graph = f_graph;
			this.faces = faces;
		}
		
		public GraphXYZ Subdivide(int iterations, IEnumerable<XYZ> fixed_v, out List<iFace> faces)
		{

            subdivided_v_graph = v_graph.Copy();

            ComputeFacePoints();
			MapEdgesToFaces();
			ComputeEdgePoints();

			UpdatePoints(new HashSet<int>(fixed_v.Select(p => v_graph.NodeIndex(p))));

			faces = ComputeNewFaces();

			return subdivided_v_graph;
		}

		private void ComputeFacePoints()
		{
			face_centers = new XYZ[faces.Length];

			for (int i = 0; i < faces.Length; i++)
			{
				iFace face = faces[i];

				if (face.IsTriangle) 
				{ 
					face_centers[i] = (v_graph.GetNode(face.A) + v_graph.GetNode(face.B) + v_graph.GetNode(face.C)) / 3; 
				}
				else 
				{ 
					face_centers[i] = (v_graph.GetNode(face.A) + v_graph.GetNode(face.B) + v_graph.GetNode(face.C) + v_graph.GetNode(face.D)) / 4;
				}

				subdivided_v_graph.TryAddNode(face_centers[i]); 	
			}
        }

		private void MapEdgesToFaces()
		{
			// Find face edges
			edge_faces = new Dictionary<int, HashSet<int>>();
			edge_map = new Dictionary<int, iEdge>();

			for (int i = 0; i < faces.Length; i++)
			{
				iEdge[] edges = faces[i].GetEdges();

				for (int j = 0; j < edges.Length; j++)
				{
					int hash = Spatial.ComputeIndexHash(edges[j]);
					if(!edge_faces.ContainsKey(hash)){edge_faces.Add(hash,new HashSet<int>());}

					edge_faces[hash].Add(i);

					if (edge_faces[hash].Count == 3)
					{
						throw new Exception("Non manifold edge detected.");
					}

					if (!edge_map.ContainsKey(hash)) { edge_map.Add(hash, edges[j]); }			
				}
			}
		}

		private List<iFace> ComputeNewFaces()
		{
			List<iFace> new_faces = new List<iFace>();

			for (int i = 0; i < faces.Length; i++)
			{
				iEdge[] edges = faces[i].GetEdges();

				int previous_hash = Spatial.ComputeIndexHash(edges[edges.Length - 1]);

				for (int j = 0; j < edges.Length; j++)
				{
					int e_hash = Spatial.ComputeIndexHash(edges[j]);
					new_faces.Add(new iFace(edges[j].id_a, subdivided_v_graph.NodeIndex(edge_points[e_hash]), subdivided_v_graph.NodeIndex(face_centers[i]), subdivided_v_graph.NodeIndex(edge_points[previous_hash])));
					previous_hash = e_hash;
				}
			}

			return new_faces;
		}

		private void ComputeEdgePoints()
		{			
			// Compute Points
			edge_points = new Dictionary<int, XYZ>(edge_map.Count);

			foreach (var pair in edge_faces)
			{
				XYZ e_avg = (v_graph.GetNode(edge_map[pair.Key].id_a) + v_graph.GetNode(edge_map[pair.Key].id_b)) / 2.0;

				if(pair.Value.Count == 2)
				{
					XYZ f_avg = new XYZ(0, 0 ,0);

					foreach (int id in pair.Value){ f_avg += face_centers[id]; }

					e_avg = (e_avg + f_avg / 2) / 2;
				}

				edge_points.Add(pair.Key, e_avg);
				subdivided_v_graph.TryAddNode(e_avg);
			}
			
		}

		private void UpdatePoints(HashSet<int> fixed_v)
		{
			for (int i = 0; i < v_graph.NodeCount; i++)
			{
                if (fixed_v.Contains(i))
                {
					continue;
                }

                HashSet<int> connected = v_graph.GetConnectedNodes(i);
				int n = connected.Count;

				XYZ[] node_edge_points= new XYZ[n];
				List<iEdge> naked_edges = new List<iEdge>();
				HashSet<int> node_face_hash = new HashSet<int>();
				List<XYZ> node_face_points = new List<XYZ>(n);

				int iterator = 0;
				// Get adjacent faces and edges
				foreach (int id in connected)
				{
					node_edge_points[iterator] = (v_graph.GetNode(i) + v_graph.GetNode(id)) / 2;

					var f_edges = edge_faces[Spatial.ComputeIndexHash(i, id)];

					if(f_edges.Count == 1)
					{
						naked_edges.Add(new iEdge(i, id));
					}

                    foreach (int f_id in f_edges)
					{
						if(node_face_hash.Add(face_centers[f_id].SpatialHash))
						{ 
							node_face_points.Add(face_centers[f_id]); 
						}
					}
					
					iterator++;
				}

				if(naked_edges.Count == 0)
				{
					// Internal point
                    subdivided_v_graph.TryUpdateNode(i, (XYZ.Average(node_face_points) + XYZ.Average(node_edge_points) * 2 + v_graph.GetNode(i) * (n - 3)) / n);
                }
				else if(node_face_points.Count == 1)
				{
					// Corner point
					subdivided_v_graph.TryUpdateNode(i, (v_graph.GetNode(naked_edges[0].id_b) + v_graph.GetNode(naked_edges[1].id_b) + v_graph.GetNode(i) * 6.0) / 8.0);
				}
				else
				{
                    // Edge point
                    subdivided_v_graph.TryUpdateNode(i, v_graph.GetNode(naked_edges[0].id_b) / 8 + v_graph.GetNode(naked_edges[1].id_b) / 8 + v_graph.GetNode(i) * 0.75);
                }			
 			}
		}
	}
}

