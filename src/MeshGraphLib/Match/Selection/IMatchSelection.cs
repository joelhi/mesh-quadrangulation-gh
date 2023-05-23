using System;
using System.Collections.Generic;
using MeshGraphLib.Core;

namespace MeshGraphLib.Match.Selection
{
	public interface IMatchSelection
	{
		iEdge PickMatching(IEnumerable<iEdge> edges, GraphXYZ graph, out int[] remaining_nodes);
	}
}

