using System;
namespace MeshGraphLib.Core
{
    public struct iEdge
    {
        public int id_a;
        public int id_b;

        public iEdge()
        {
            id_a = id_b = -1;
        }

        public iEdge(int id_a, int id_b)
        {
            this.id_a = id_a;
            this.id_b = id_b;
        }

        public bool IsIdentical(iEdge e, bool directed = false)
        {
            if(directed)
            {
                return (e.id_a == id_a) && (e.id_b == id_b);
            }
            else
            {
                return ((e.id_a == id_a) && (e.id_b == id_b)) || ((e.id_a == id_b) && (e.id_b == id_a));
            }
        }
    }

    public struct EdgeXYZ
    {
        XYZ xyz_a;
        XYZ xyz_b;

        public XYZ A => this.xyz_a;

        public XYZ B => this.xyz_b;

        public EdgeXYZ()
        {
            xyz_a = xyz_b = new XYZ();
        }

        public EdgeXYZ(XYZ xyz_a, XYZ xyz_b)
        {
            this.xyz_a = xyz_a;
            this.xyz_b = xyz_b;
        }
    }
}