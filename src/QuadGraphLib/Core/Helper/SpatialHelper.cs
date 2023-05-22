using System;
using System.Collections.Generic;
namespace QuadGraphLib.Core.Helper
{

    public static class Spatial
    {

        public static void SetGlobalTol(double val)
        {
            global_spatial_tolerance = val;
        }

        public static double GetGlobalTol() => global_spatial_tolerance; 

        private static double global_spatial_tolerance = 0.001;

        public static int ComputeSpatialHash(double x, double y, double z)
        {
            double multiplier = 1 / global_spatial_tolerance;
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

        public static int ComputeIndexHash(int id_a, int id_b)
        {
            if(id_a >= id_b)
            {
                return (23 * 37 + id_a) * 37 + id_b;
            }
            else
            {
                return (23 * 37 + id_b) * 37 + id_a;
            }
        }

        public static int ComputeIndexHash(int[] ids)
        {
            Array.Sort(ids);

            int hash = 23;
            for (int i = 0; i < ids.Length; i++)
            {
                hash = hash * 37 + ids[i];
            }

            return hash;
        }

        public static double Length(this XYZ xyz) => Math.Sqrt(Math.Pow(xyz.x,2) + Math.Pow(xyz.y,2) + Math.Pow(xyz.z,2));

        public static double Length(this EdgeXYZ edge) => DistanceTo(edge.A, edge.B);
        
        public static double DistanceTo(this XYZ a, XYZ b) => (b- a).Length();
    }

}