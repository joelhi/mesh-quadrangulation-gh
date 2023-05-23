using System;
using System.Collections.Generic;
using MeshGraphLib.Core;

namespace MeshGraphLib.Walk.Interfaces
{
    public interface IWalk
    {
        GraphXYZ Walk(IEnumerable<int> start_indices);
    }
}

    

