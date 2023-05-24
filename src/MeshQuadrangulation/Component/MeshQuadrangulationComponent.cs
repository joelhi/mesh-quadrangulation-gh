﻿using System;
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
  public class MeshQuadrangulationComponent : GH_Component
  {
    /// <summary>
    /// Each implementation of GH_Component must provide a public 
    /// constructor without any arguments.
    /// Category represents the Tab in which the component will appear, 
    /// Subcategory the panel. If you use non-existing tab or panel names, 
    /// new tabs/panels will automatically be created.
    /// </summary>
    public MeshQuadrangulationComponent()
      : base("Mesh Quadrangulation", "m_quadr",
        "Quadrangulate a tri mesh using a matching algorithm",
        "Mesh", "Quadrangulation")
    {
    }

    /// <summary>
    /// Registers all the input parameters for this component.
    /// </summary>
    protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
    {
      pManager.AddMeshParameter("Mesh","M","Mesh to quadrangulate",GH_ParamAccess.item);
    }

    /// <summary>
    /// Registers all the output parameters for this component.
    /// </summary>
    protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
    {
      pManager.AddMeshParameter("Mesh","M","Quadrangulated Mesh",GH_ParamAccess.item);
      pManager.AddIntegerParameter("Sources","S","Sources for the search.",GH_ParamAccess.list);
    }

    /// <summary>
    /// This is the method that actually does the work.
    /// </summary>
    /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
    /// to store data in output parameters.</param>
    protected override void SolveInstance(IGH_DataAccess DA)
    {
        Mesh m = new Mesh();

        DA.GetData(0, ref m);

        GraphXYZ f_graph = m.ToFaceGraph();
        iFace[] faces = m.ToFaces();

        Quadrangulation q = new Quadrangulation(f_graph, faces, new EdgeLengthSelection());

        iFace[] q_faces = q.Quadrangulate(new int[1]{5});

        Mesh q_m = new Mesh();

        q_m.Vertices.AddVertices(m.Vertices);
        q_m.Faces.AddFaces(q_faces.ToRhino());

        DA.SetData(0, q_m);

    }

    /// <summary>
    /// Provides an Icon for every component that will be visible in the User Interface.
    /// Icons need to be 24x24 pixels.
    /// You can add image files to your project resources and access them like this:
    /// return Resources.IconForThisComponent;
    /// </summary>
    protected override System.Drawing.Bitmap Icon => null;

    /// <summary>
    /// Each component must have a unique Guid to identify it. 
    /// It is vital this Guid doesn't change otherwise old ghx files 
    /// that use the old ID will partially fail during loading.
    /// </summary>
    public override Guid ComponentGuid => new Guid("3823e604-a322-4cee-aea9-7ee1fa3bb6fd");
  }
}
