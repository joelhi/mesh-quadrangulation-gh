using System;
namespace QuadGraphLib.Core
{
    public struct Edge
    {
        int id_a;
        int id_b;

        public Edge()
        {
            id_a = id_b = -1;
        }

        public Edge(int id_a, int id_b)
        {
            this.id_a = id_a;
            this.id_b = id_b;
        }
    }
}