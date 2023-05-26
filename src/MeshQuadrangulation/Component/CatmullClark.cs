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
  public class CatmullClarkComponent : GH_Component
  {
    /// <summary>
    /// Each implementation of GH_Component must provide a public 
    /// constructor without any arguments.
    /// Category represents the Tab in which the component will appear, 
    /// Subcategory the panel. If you use non-existing tab or panel names, 
    /// new tabs/panels will automatically be created.
    /// </summary>
    public CatmullClarkComponent()
      : base("Catmull Clark", "catmull",
        "Subdivide a mesh using the catmull clark algorithm",
        "Mesh", "Quadrangulation")
    {
    }

    protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
    {
      pManager.AddMeshParameter("Mesh","M","Mesh to subdivide",GH_ParamAccess.item);
      pManager.AddPointParameter("Fixed Vertices","fix","Vertices to fix during the process",GH_ParamAccess.list);

      pManager[1].Optional = true;
    }

    protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
    {
      pManager.AddMeshParameter("Mesh","M","Subdivided Mesh",GH_ParamAccess.item);
    }

    protected override void SolveInstance(IGH_DataAccess DA)
    {
        Mesh m = new Mesh();
        List<Point3d> fix = new List<Point3d>();

        DA.GetData(0, ref m);
        DA.GetDataList(1, fix);

        GraphXYZ f_graph = m.ToFaceGraph();
        iFace[] faces = m.ToFaces();
        GraphXYZ v_graph = m.ToVertexGraph();

        CatmullClark cc = new CatmullClark(v_graph, f_graph, faces);

        var result = cc.Subdivide(1, fix.Select(p => p.ToXYZ()), out List<iFace> subdivided_faces);
        Mesh q_m = new Mesh();

        q_m.Vertices.AddVertices(result.GetNodes().ToRhino());
        q_m.Faces.AddFaces(subdivided_faces.ToArray().ToRhino());

        q_m.Normals.ComputeNormals();

        DA.SetData(0, q_m);

    }

    protected override System.Drawing.Bitmap Icon => null;

    public override Guid ComponentGuid => new Guid("3823e504-a322-4cae-aea9-7e81fa3cc2fd");
  }
}
