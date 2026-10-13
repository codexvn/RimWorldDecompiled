using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002797 RID: 10135
	public class RitualPosition_OnInteractionCell : RitualPosition_Cells
	{
		// Token: 0x0600DE01 RID: 56833 RVA: 0x00415551 File Offset: 0x00413751
		public override void FindCells(List<IntVec3> cells, Thing thing, CellRect rect, IntVec3 spot, Rot4 rotation)
		{
			cells.Add(thing.InteractionCell + this.offset);
		}

		// Token: 0x0600DE02 RID: 56834 RVA: 0x0041556C File Offset: 0x0041376C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<IntVec3>(ref this.offset, "offset", default(IntVec3), false);
		}

		// Token: 0x040095AF RID: 38319
		public IntVec3 offset = IntVec3.Zero;
	}
}
