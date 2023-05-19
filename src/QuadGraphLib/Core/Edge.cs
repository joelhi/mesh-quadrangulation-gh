using System;
namespace QuadGraphLib.Core
{
    public struct iEdge
    {
        int id_a;
        int id_b;

        public iEdge()
        {
            id_a = id_b = -1;
        }

        public iEdge(int id_a, int id_b)
        {
            this.id_a = id_a;
            this.id_b = id_b;
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