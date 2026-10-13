using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000462 RID: 1122
	public class RoadInfo : MapComponent
	{
		// Token: 0x06001D6B RID: 7531 RVA: 0x0009491D File Offset: 0x00092B1D
		public RoadInfo(Map map) : base(map)
		{
		}

		// Token: 0x06001D6C RID: 7532 RVA: 0x00094931 File Offset: 0x00092B31
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<IntVec3>(ref this.roadEdgeTiles, "roadEdgeTiles", LookMode.Undefined, Array.Empty<object>());
		}

		// Token: 0x0400198A RID: 6538
		public List<IntVec3> roadEdgeTiles = new List<IntVec3>();
	}
}
