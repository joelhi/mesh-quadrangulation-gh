using System;
using MeshGraphLib.Core.Helper;

namespace MeshGraphLib.Core
{
	public struct XYZ
	{
		public double x;

		public double y;

		public double z;

		public XYZ()
		{
			this.x = this.y = this.z = 0;
		}

		public XYZ(double x, double y)
		{
			this.x = x;
			this.y = y;
			this.z = 0;
		}

        public XYZ(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

		public XYZ INVALID => new XYZ(double.NaN, double.NaN, double.NaN);

		public bool IsValid => !(double.IsNaN(this.x) || double.IsNaN(this.y) || double.IsNaN(this.z));

        public int SpatialHash => Spatial.ComputeSpatialHash(this.x, this.y, this.z);

		public static XYZ operator+ (XYZ a, XYZ b) => new XYZ(a.x + b.x, a.y + b.y, a.z + b.z);

		public static XYZ operator- (XYZ a, XYZ b) => new XYZ(a.x - b.x, a.y - b.y, a.z - b.z);

		public static double operator* (XYZ a, XYZ b) => a.x * b.x + a.y * b.y + a.z * b.z;

		public static XYZ operator* (XYZ xyz, double val) => new XYZ(xyz.x * val, xyz.y * val, xyz.z * val);

		public static XYZ operator/ (XYZ xyz, double val) => new XYZ(xyz.x / val, xyz.y / val, xyz.z / val);

    }
}

