using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020029B1 RID: 10673
	public interface ISlotGroup
	{
		// Token: 0x1700242A RID: 9258
		// (get) Token: 0x0600EB35 RID: 60213
		StorageSettings Settings { get; }

		// Token: 0x1700242B RID: 9259
		// (get) Token: 0x0600EB36 RID: 60214
		IEnumerable<Thing> HeldThings { get; }

		// Token: 0x1700242C RID: 9260
		// (get) Token: 0x0600EB37 RID: 60215
		StorageGroup StorageGroup { get; }

		// Token: 0x1700242D RID: 9261
		// (get) Token: 0x0600EB38 RID: 60216
		List<IntVec3> CellsList { get; }

		// Token: 0x1700242E RID: 9262
		// (get) Token: 0x0600EB39 RID: 60217
		string GroupingLabel { get; }

		// Token: 0x1700242F RID: 9263
		// (get) Token: 0x0600EB3A RID: 60218
		int GroupingOrder { get; }
	}
}
