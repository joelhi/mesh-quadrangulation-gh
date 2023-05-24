using System;
using System.Collections.Generic;
using MeshGraphLib.Core;
using MeshGraphLib.Match;
using MeshGraphLib.Match.Selection;
using Rhino.Geometry;

namespace MeshGraphLib.Process
{
    public class Quadrangulation
    {
        private BFSMatching matching;

        private iFace[] faces;

        public Quadrangulation(GraphXYZ face_graph, iFace[] faces, IMatchSelection selection_criteria)
        {
            this.matching = new BFSMatching(face_graph, selection_criteria);
            this.faces = faces;
        }

        public iFace[] Quadrangulate(int[] sources)
        {
            var matchings = matching.ComputeMatchings(sources, out List<int> singles);
            return MergeAllFaces(this.faces, matchings, singles);
        }

        public iFace[] MergeAllFaces(iFace[] faces, List<iEdge> matchings, List<int> singles)
        {
            iFace[] merged = new iFace[matchings.Count + singles.Count];

            for (int i = 0; i < matchings.Count; i++)
            {
                merged[i] = MergeTrianglesToQuads(faces[matchings[i].id_a], faces[matchings[i].id_b]);
            }

            for (int i = 0; i < singles.Count; i++)
            {
                merged[matchings.Count + i] = faces[singles[i]];
            }

            return merged;
        }

        public iFace MergeTrianglesToQuads(iFace A, iFace B)
        {

            iFace final = iFace.Unset;

            if (A.IsTriangle && B.IsTriangle)
            {
                int[] a_arr = new int[3] { A.A, A.B, A.C };
                int[] b_arr = new int[3] { B.A, B.B, B.C };

                List<int> shared = new List<int>();
                HashSet<int> sharedIndexA = new HashSet<int>();
                HashSet<int> sharedIndexB = new HashSet<int>();

                for (int i = 0; i < 3; i++)
                {
                    int index = -1;

                    for (int j = 0; j < 3; j++)
                    {
                        if (b_arr[j] == a_arr[i]) { index = j; }
                    }

                    if (index >= 0)
                    {
                        sharedIndexA.Add(i); sharedIndexB.Add(index); shared.Add(a_arr[i]);
                    }
                }

                int notSharedB = -1;
                for (int i = 0; i < 3; i++)
                {
                    if (!sharedIndexB.Contains(i)) { notSharedB = b_arr[i]; }
                }

                if (sharedIndexA.Contains(0) && sharedIndexA.Contains(1))
                {
                    final.A = a_arr[0]; final.B = notSharedB; final.C = a_arr[1]; final.D = a_arr[2];
                }
                else if (sharedIndexA.Contains(0))
                {
                    final.A = a_arr[0]; final.B = a_arr[1]; final.C = a_arr[2]; final.D = notSharedB;
                }
                else
                {
                    final.A = a_arr[0]; final.B = a_arr[1]; final.C = notSharedB; final.D = a_arr[2];
                }

            }
            else { throw new Exception("This can only be done for triangles."); }

            return final;
        }
    }
}