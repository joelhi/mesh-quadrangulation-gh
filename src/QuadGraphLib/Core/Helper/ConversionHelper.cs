using System;
using System.Collections.Generic;
using Rhino.Geometry;
using System.Linq;

namespace QuadGraphLib.Core.Helper
{
    public static class Conversions
    {

        public static GraphXYZ ToVertexGraph(this Mesh mesh)
        {

            // Mesh cleanup
            mesh.Compact();
            mesh.Vertices.CombineIdentical(true, true);
            mesh.Vertices.CullUnused();

            GraphXYZ graph = new GraphXYZ();

            graph.TryAddNodes(mesh.Vertices.ToPoint3dArray().ToXYZ());

            for (int i = 0; i < mesh.Vertices.Count; i++)
            {
                mesh.Vertices.GetConnectedVertices(i).Select(j => graph.TryAddEdge(i, j));
            }

            return graph;
        }

        public static GraphXYZ ToFaceGraph(this Mesh mesh)
        {
            // Mesh cleanup
            mesh.Compact();
            mesh.Vertices.CombineIdentical(true, true);
            mesh.Vertices.CullUnused();

            GraphXYZ graph = new GraphXYZ();

            // Add face center nodess to graph
            graph.TryAddNodes(
                mesh.Faces.Select(
                    face => GetFaceCenter(face, mesh)));

            // Add edges for adjacent faces.
            for (int i = 0; i < mesh.Faces.Count; i++)
            {
                mesh.Faces.AdjacentFaces(i).Select(id => graph.TryAddEdge(i, id));
            }

            return graph;
        }

        private static XYZ GetFaceCenter(MeshFace face, Mesh mesh)
        {
            if(face.IsTriangle)
            {
                return (mesh.Vertices[face[0]] + mesh.Vertices[face[1]] + mesh.Vertices[face[2]]).ToXYZ() / 3;
            }
            
            return (mesh.Vertices[face[0]] + mesh.Vertices[face[1]] + mesh.Vertices[face[2]] + mesh.Vertices[face[3]]).ToXYZ() / 3;
        }

        public static Point3d ToRhino(this XYZ node) => new Point3d(node.x, node.y, node.z);

        public static Point3d[] ToRhino(this XYZ[] nodes)
        {
            Point3d[] rh_pts = new Point3d[nodes.Length];

            unsafe
            {
                int size = nodes.Length * sizeof(XYZ);

                fixed(void* nd_ptr = &nodes[0])
                {
                    fixed(void* pt_ptr = &rh_pts[0])
                    {
                        Buffer.MemoryCopy(nd_ptr, pt_ptr, size, size);
                    }
                }
            }

            return rh_pts;
        }

        public static XYZ ToXYZ(this Point3d pt) => new XYZ(pt.X, pt.Y, pt.Z);

        public static XYZ ToXYZ(this Point3f pt) => new XYZ(pt.X, pt.Y, pt.Z);

        public static XYZ[] ToXYZ(this Point3d[] rh_pts)
        {
            XYZ[] nodes = new XYZ[rh_pts.Length];

            unsafe
            {
                int size = nodes.Length * sizeof(XYZ);

                fixed(void* nd_ptr = &nodes[0])
                {
                    fixed(void* pt_ptr = &rh_pts[0])
                    {
                        Buffer.MemoryCopy(pt_ptr, nd_ptr, size, size);
                    }
                }
            }

            return nodes;
        }

        public static EdgeXYZ[] ToEdgeXYZ(this Line[] rh_lines)
        {
            EdgeXYZ[] edges = new EdgeXYZ[rh_lines.Length];

            unsafe
            {
                int size = edges.Length * sizeof(EdgeXYZ);

                fixed(void* nd_ptr = &edges[0])
                {
                    fixed(void* pt_ptr = &rh_lines[0])
                    {
                        Buffer.MemoryCopy(pt_ptr, nd_ptr, size, size);
                    }
                }
            }

            return edges;
        }

        public static Line[] ToRhino(this EdgeXYZ[] edges)
        {
            Line[] rh_lns = new Line[edges.Length];

            unsafe
            {
                int size = edges.Length * sizeof(Line);

                fixed(void* e_ptr = &edges[0])
                {
                    fixed(void* ln_ptr = &rh_lns[0])
                    {
                        Buffer.MemoryCopy(e_ptr, ln_ptr, size, size);
                    }
                }
            }

            return rh_lns;
        }
    }
}