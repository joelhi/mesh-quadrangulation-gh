using System;
using System.Collections.Generic;
using MeshGraphLib.Core;
namespace MeshGraphLib.Algorithms
{
    public class LaplacianSmooth
    {
        private HashSet<int> fixed_v;
        private GraphXYZ vertex_graph;
        public LaplacianSmooth(GraphXYZ vertex_graph, IEnumerable<XYZ> fixed_points)
        {
            this.vertex_graph = vertex_graph;
            fixed_v = new HashSet<int>();

            foreach (XYZ p in fixed_points) { fixed_v.Add(vertex_graph.NodeIndex(p)); }
        }

        public GraphXYZ Smooth(int iterations)
        {
            XYZ[] smooth_nodes = new XYZ[vertex_graph.NodeCount];

            XYZ[] nodes = vertex_graph.GetNodes();

            for (int iter = 0; iter < iterations; iter++)
            {
                for (int i = 0; i < vertex_graph.NodeCount; i++)
                {
                    if (fixed_v.Contains(i))
                    {
                        smooth_nodes[i] = nodes[i];
                        continue;
                    }

                    XYZ avg = new XYZ(0, 0, 0);
                    foreach (int id in vertex_graph.GetConnectedNodes(i)){ avg += nodes[id]; }

                    smooth_nodes[i] = avg / vertex_graph.GetConnectedNodes(i).Count;
                }

                Array.Copy(smooth_nodes, nodes, smooth_nodes.Length);
            }


            GraphXYZ smooth_graph = new GraphXYZ();
            smooth_graph.TryAddNodes(smooth_nodes);

            foreach (iEdge e in vertex_graph.GetEdgesConnectivity()) { smooth_graph.TryAddEdge(e.id_a, e.id_b); }

            return smooth_graph;
        }
    }
}

