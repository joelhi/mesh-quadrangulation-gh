using System;
namespace MeshGraphLib.Core
{


	public struct iFace
	{
		public int A;
		public int B;
		public int C;
		public int D;

		public iFace(int A, int B, int C)
		{
			this.A = A;
			this.B = B;
			this.C = C;
			this.D = C;
		}

        public iFace(int A, int B, int C, int D)
        {
            this.A = A;
            this.B = B;
            this.C = C;
            this.D = D;
        }

        public bool IsTriangle => (this.D == this.C);

		public static iFace Unset => new iFace(-1, -1, -1);
	}
}

