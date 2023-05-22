using System;
using Rhino.Geometry;

namespace QuadGraphLib.Core.Helper
{
    public static class Conversions
    {

        public static UndirectedGraphXYZ ToGraph(this Mesh mesh)
        {
            mesh.Compact();

            XYZ[] nodes = mesh.Vertices.ToPoint3dArray().ToXYZ();

            
            
        }

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

        public static Point3d[] ToEdgeXYZ(this XYZ[] nodes)
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
    }
}