using System;
using Rhino.Geometry;

namespace QuadGraphLib.Core.Helper
{
    public static class ConversionHelper
    {
        public static Point3d[] XYZToRhino(this XYZ[] nodes)
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

        public static XYZ[] RhinoToXYZ(this Point3d[] rh_pts)
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
    }
}