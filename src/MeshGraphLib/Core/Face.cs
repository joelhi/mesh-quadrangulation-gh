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

        public int[] AsArray()
        {
            if(IsTriangle){ return new int[3] {A, B , C};}
            else { return new int[4] {A, B , C, D};}
        }

        public iEdge[] GetEdges()
        {
            if(IsTriangle)
            {
                iEdge[] edges = new iEdge[3];

                edges[0] = new iEdge(A, B);
                edges[1] = new iEdge(B, C);
                edges[2] = new iEdge(C, A);

                return edges;
            }
            else
            {
                iEdge[] edges = new iEdge[4];

                edges[0] = new iEdge(A, B);
                edges[1] = new iEdge(B, C);
                edges[2] = new iEdge(C, D);
                edges[3] = new iEdge(D, A);

                return edges;
            }
        }

        public static iFace Unset => new iFace(-1, -1, -1);

        private bool HasSharedEdge(iFace B, out iEdge edge)
		{
			foreach (iEdge e1 in GetEdges())
			{
				foreach (iEdge e2 in B.GetEdges())
				{
					if(e1.IsIdentical(e2)) 
                    {
                        edge =  e1;
                        return true;
                    }
				}
			}

			edge = new iEdge(-1 ,-1);
            return false;
		}
    }
}

