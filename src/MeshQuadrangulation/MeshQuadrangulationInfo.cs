using System;
using System.Drawing;
using Grasshopper;
using Grasshopper.Kernel;

using System;

using MeshGraphLib;


namespace MeshQuadrangulation
{
  public class MeshQuadrangulationInfo : GH_AssemblyInfo
  {
    public override string Name => "MeshQuadrangulation";

    //Return a 24x24 pixel bitmap to represent this GHA library.
    public override Bitmap Icon => null;

    //Return a short string describing the purpose of this GHA library.
    public override string Description => "";

    public override Guid Id => new Guid("629d027d-b877-4761-a666-31df20689368");

    //Return a string identifying you or your company.
    public override string AuthorName => "";

    //Return a string representing your preferred contact details.
    public override string AuthorContact => "";
  }
}
