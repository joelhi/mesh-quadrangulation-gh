using System;
using System.Collections.Generic;
using System.Linq;

using Grasshopper;
using Grasshopper.Kernel;
using Rhino.Geometry;

using MeshGraphLib.Core.Helper;
using MeshGraphLib.Core;
using MeshGraphLib.Algorithms.Walk;
using MeshGraphLib.Algorithms.Walk.Interfaces;
using MeshGraphLib.Algorithms.Match;
using MeshGraphLib.Algorithms.Match.Selection;
using MeshGraphLib.Algorithms;

namespace MeshQuadrangulation
{
    public class LaplacianSmoothComponent : GH_Component
    {

        public LaplacianSmoothComponent()
        : base("Laplacian Smooth", "m_lpsmth",
        "Smooth mesh using a laplacian",
        "Mesh", "Quadrangulation")
        {
        }
        
        public override Guid ComponentGuid => new Guid("3824e604-a528-4cee-aea9-7be1fa3db6fd");

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddMeshParameter("Mesh","M","Mesh to smooth",GH_ParamAccess.item);
            pManager.AddPointParameter("Fixed Vertices","fix","Vertices to fix during the smoothing process",GH_ParamAccess.list);
            pManager.AddIntegerParameter("Iterations","iter","Number of smoothing iterations",GH_ParamAccess.item, 1);
            pManager.AddNumberParameter("Tolerance", "tol", "Tolerance for point hashing", GH_ParamAccess.item, 1e-4);

            pManager[1].Optional = true;
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddMeshParameter("Mesh","M","Smooth mesh",GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Mesh m = new Mesh();
            List<Point3d> fix = new List<Point3d>();
            int iter = 1;
            double tol = 1e-4;

            DA.GetData(0, ref m);
            DA.GetDataList(1, fix);
            DA.GetData(2, ref iter);
            DA.GetData(3, ref tol);

            double old_tol = Spatial.GetGlobalTol();

            Spatial.SetGlobalTol(tol);

            GraphXYZ v_graph = m.ToVertexGraph();

            LaplacianSmooth smoother = new LaplacianSmooth(v_graph, fix.Select(p => p.ToXYZ()));

            GraphXYZ smooth_graph = smoother.Smooth(iter);

            Mesh smooth_m = new Mesh();

            smooth_m.Vertices.AddVertices(smooth_graph.GetNodes().ToRhino());
            smooth_m.Faces.AddFaces(m.Faces);

            smooth_m.Normals.ComputeNormals();

            DA.SetData(0, smooth_m);

            Spatial.SetGlobalTol(old_tol);
        }
    }
}