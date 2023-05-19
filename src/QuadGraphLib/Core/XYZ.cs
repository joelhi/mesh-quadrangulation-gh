using System;
using QuadGraphLib.Core.Helper;

namespace QuadGraphLib.Core
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

		public double SpatialHash => SpatialHelper.compute_spatial_hash(this.x, this.y, this.z);
    }
}

