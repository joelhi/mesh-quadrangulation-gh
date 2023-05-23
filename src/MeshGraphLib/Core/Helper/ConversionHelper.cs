using System;
using System.Collections.Generic;
using Rhino.Geometry;
using System.Linq;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MeshGraphLib.Core.Helper
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

            if (mesh.Faces.Count != graph.NodeCount)
            {
                throw new Exception("Falied to add all faces to graph. Please double check tolerance.");
            }

            // Add edges for adjacent faces.
            for (int i = 0; i < mesh.Faces.Count; i++)
            {
                int[] adjacent_faces = mesh.Faces.AdjacentFaces(i);

                for (int j = 0; j < adjacent_faces.Length; j++)
                {
                    graph.TryAddEdge(i, adjacent_faces[j]);
                }
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

        public static iFace[] ToFaces(this Mesh mesh)
        {
            MeshFace[] rh_faces = mesh.Faces.ToArray();

            iFace[] faces = new iFace[rh_faces.Length];

            unsafe
            {
                int size = faces.Length * sizeof(MeshFace);

                fixed (void* f_ptr = &faces[0])
                {
                    fixed (void* r_ptr = &rh_faces[0])
                    {
                        Buffer.MemoryCopy(r_ptr, f_ptr, size, size);
                    }
                }
            }

            return faces;
        }

        public static MeshFace[] ToRhino(this iFace[] faces)
        {

            MeshFace[] rh_faces = new MeshFace[faces.Length];

            unsafe
            {
                int size = faces.Length * sizeof(MeshFace);

                fixed (void* f_ptr = &faces[0])
                {
                    fixed (void* r_ptr = &rh_faces[0])
                    {
                        Buffer.MemoryCopy(f_ptr, r_ptr, size, size);
                    }
                }
            }

            return rh_faces;
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