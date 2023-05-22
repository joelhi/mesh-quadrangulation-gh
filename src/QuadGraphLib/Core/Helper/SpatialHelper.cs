using System;
using System.Collections.Generic;
namespace QuadGraphLib.Core.Helper
{

    public static class Spatial
    {
        public static int ComputeSpatialHash(double x, double y, double z, double tol = 0.001)
        {
            double multiplier = 1 / tol;
            int s_hash = 23;

            s_hash = s_hash * 37 + (int)(x * multiplier);
            s_hash = s_hash * 37 + (int)(y * multiplier);
            s_hash = s_hash * 37 + (int)(z * multiplier);

            return s_hash;
        }

        public static List<XYZ> GetUniqueNodes(IEnumerable<EdgeXYZ> edges)
        {
            HashSet<int> hashes = new HashSet<int>();
            List<XYZ> unique_nodes = new List<XYZ>();

            foreach (EdgeXYZ edge in edges)
            {
                if(hashes.Add(edge.A.SpatialHash))
                {
                    unique_nodes.Add(edge.A);
                }

                if(hashes.Add(edge.B.SpatialHash))
                {
                    unique_nodes.Add(edge.B);
                }
            }

            return unique_nodes;
        }
    }

}