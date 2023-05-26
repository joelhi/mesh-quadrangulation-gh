using System;
using System.Collections.Generic;
using Rhino.Geometry;

namespace MeshGraphLib.Core
{
    public class GraphXYZ
    {
        private List<XYZ> nodes_xyz { get; set; }

        private Dictionary<int, int> nodes_map { get; set; }

        private List<HashSet<int>> nodes_conn { get; set; }

        public bool IsDirected { get; private set; }

        public GraphXYZ(bool directed = false)
        {
            nodes_xyz = new List<XYZ>();
            nodes_map = new Dictionary<int, int>();
            nodes_conn = new List<HashSet<int>>();

            this.IsDirected = directed;
        }

        public bool HasNode(XYZ node) => nodes_map.ContainsKey(node.SpatialHash);

        public int NodeIndex(XYZ node) => nodes_map[node.SpatialHash];

        public bool HasEdge(EdgeXYZ edge) => HasEdge(edge.A, edge.B);

        public bool HasEdge(XYZ node_a, XYZ node_b) => HasEdge(NodeIndex(node_a), NodeIndex(node_b));

        public bool HasEdge(int id_a, int id_b)
        {
            if (id_a >= nodes_xyz.Count || id_b >= nodes_xyz.Count) { return false; }

            if (nodes_conn[id_a].Contains(id_b)) { return true; }

            if (IsDirected) { return false; }

            return nodes_conn[id_b].Contains(id_a);
        }

        public HashSet<int> GetConnectedNodes(int id) => nodes_conn[id];

        public int NodeCount => nodes_xyz.Count;

        public List<XYZ> TryAddNodes(IEnumerable<XYZ> nodes)
        {
            List<XYZ> failed = new List<XYZ>();

            foreach (XYZ node in nodes)
            {
                if (!TryAddNode(node))
                {
                    failed.Add(node);
                }
            }

            return failed;
        }

        public bool TryAddNode(XYZ node)
        {
            if (HasNode(node))
            {
                return false;
            }

            nodes_map.Add(node.SpatialHash, nodes_xyz.Count);
            nodes_xyz.Add(node);
            nodes_conn.Add(new HashSet<int>());

            return true;
        }

        public bool TryAddEdge(EdgeXYZ edge, bool add_nodes = false) => TryAddEdge(edge.A, edge.B, add_nodes);

        public bool TryAddEdge(XYZ node_a, XYZ node_b, bool add_nodes = false)
        {
            if (add_nodes)
            {
                TryAddNode(node_a);
                TryAddNode(node_b);
            }

            return TryAddEdge(NodeIndex(node_a), NodeIndex(node_b));
        }

        public bool TryAddEdge(int id_a, int id_b)
        {
            if (HasEdge(id_a, id_b)) { return false; }

            if (id_a >= nodes_xyz.Count || id_b >= nodes_xyz.Count) { return false; }

            nodes_conn[id_a].Add(id_b);

            if (IsDirected) { return true; }

            nodes_conn[id_b].Add(id_a);

            return true;
        }

        public EdgeXYZ[] GetEdges()
        {
            List<EdgeXYZ> edges = new List<EdgeXYZ>();

            HashSet<int> visited_nodes = new HashSet<int>();

            for (int i = 0; i < nodes_xyz.Count; i++)
            {
                visited_nodes.Add(i);

                foreach (int id in nodes_conn[i])
                {
                    if (visited_nodes.Contains(id)) { continue; }

                    edges.Add(new EdgeXYZ(nodes_xyz[i], nodes_xyz[id]));
                }
            }

            return edges.ToArray();
        }

        public iEdge[] GetEdgesConnectivity()
        {
            List<iEdge> edges = new List<iEdge>();

            HashSet<int> visited_nodes = new HashSet<int>();

            for (int i = 0; i < nodes_xyz.Count; i++)
            {
                visited_nodes.Add(i);

                foreach (int id in nodes_conn[i])
                {
                    if (visited_nodes.Contains(id)) { continue; }

                    edges.Add(new iEdge(i, id));
                }
            }

            return edges.ToArray();
        }

        public XYZ[] GetNodes() => nodes_xyz.ToArray();

        public XYZ GetNode(int id) => nodes_xyz[id];

        public bool TryUpdateNode(int id, XYZ new_node)
        {
            if(id >= nodes_xyz.Count) { return false; }

            nodes_xyz[id] = new_node;

            return true;
        }

        public bool TryGetNode(int id, out XYZ node)
        {
            if (id >= nodes_xyz.Count)
            {
                node = new XYZ(0, 0, 0);
                return false;
            }

            node = nodes_xyz[id];

            return true;
        }

        public GraphXYZ Copy()
        {
            return new GraphXYZ()
            {
                nodes_xyz = new List<XYZ>(this.nodes_xyz),
                nodes_map = new Dictionary<int, int>(this.nodes_map),
                nodes_conn = new List<HashSet<int>>(this.nodes_conn),
                IsDirected = this.IsDirected
            };
        }

        public List<iFace> GetFaces()
        {
            List<iFace> faces = new List<iFace>();

            for (int i = 0; i < NodeCount; i++)
            {
                foreach (int connected_A in GetConnectedNodes(i))
                {
                    foreach (int connected_B in GetConnectedNodes(i))
                    { 
                        HashSet<int> next_connections = GetConnectedNodes(connected_A);

                        if(next_connections.Contains(i))
                        {
                            faces.Add(new iFace(i, connected_A, connected_B));
                            break;
                        }

                        foreach (int connected_C in GetConnectedNodes(i))
                        { 
                            HashSet<int> next_connections_b = GetConnectedNodes(connected_A);

                            if(next_connections.Contains(i))
                            {
                                faces.Add(new iFace(i, connected_A, connected_B, connected_C));
                                break;
                            }
                        }
                    }
                }
            }

            return faces;
        }
    }
}

