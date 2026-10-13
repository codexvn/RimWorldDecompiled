using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020005B0 RID: 1456
	public interface IPathFinderDataSource : IDisposable
	{
		// Token: 0x0600259E RID: 9630
		void ComputeAll(IEnumerable<PathRequest> requests);

		// Token: 0x0600259F RID: 9631
		bool UpdateIncrementally(IEnumerable<PathRequest> requests, List<IntVec3> cellDeltas);
	}
}
