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
  public class QuadrangulateMesh : GH_Component
  {
    /// <summary>
    /// Each implementation of GH_Component must provide a public 
    /// constructor without any arguments.
    /// Category represents the Tab in which the component will appear, 
    /// Subcategory the panel. If you use non-existing tab or panel names, 
    /// new tabs/panels will automatically be created.
    /// </summary>
    public QuadrangulateMesh()
      : base("Mesh Quadrangulation", "m_quadr",
        "Quadrangulate a tri mesh using a matching algorithm",
        "Mesh", "Quadrangulation")
    {
    }

    protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
    {
      pManager.AddMeshParameter("Mesh","M","Mesh to quadrangulate",GH_ParamAccess.item);
      pManager.AddIntegerParameter("Sources","S","Sources for the search.",GH_ParamAccess.list);

      pManager[1].Optional = true;
    }

    protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
    {
      pManager.AddMeshParameter("Mesh","M","Quadrangulated Mesh",GH_ParamAccess.item);
    }

    protected override void SolveInstance(IGH_DataAccess DA)
    {
        Mesh m = new Mesh();
        List<int> sources = new List<int>();

        DA.GetData(0, ref m);
        if(!DA.GetDataList(1, sources)){ sources.Add(0);}

        GraphXYZ f_graph = m.ToFaceGraph();
        iFace[] faces = m.ToFaces();

        Quadrangulation q = new Quadrangulation(f_graph, faces, new EdgeLengthSelection());

        iFace[] q_faces = q.Quadrangulate(sources);

        Mesh q_m = new Mesh();

        q_m.Vertices.AddVertices(m.Vertices);
        q_m.Faces.AddFaces(q_faces.ToRhino());

        q_m.Normals.ComputeNormals();

        DA.SetData(0, q_m);

    }

    protected override System.Drawing.Bitmap Icon => null;

    public override Guid ComponentGuid => new Guid("3823e604-a322-4cee-aea9-7ee1fa3bb6fd");
  }
}
