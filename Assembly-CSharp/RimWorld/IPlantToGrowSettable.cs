using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020021CF RID: 8655
	public interface IPlantToGrowSettable
	{
		// Token: 0x17001CCB RID: 7371
		// (get) Token: 0x0600BD13 RID: 48403
		Map Map { get; }

		// Token: 0x17001CCC RID: 7372
		// (get) Token: 0x0600BD14 RID: 48404
		IEnumerable<IntVec3> Cells { get; }

		// Token: 0x0600BD15 RID: 48405
		ThingDef GetPlantDefToGrow();

		// Token: 0x0600BD16 RID: 48406
		void SetPlantDefToGrow(ThingDef plantDef);

		// Token: 0x0600BD17 RID: 48407
		bool CanAcceptSowNow();
	}
}
