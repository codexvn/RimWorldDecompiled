using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02003525 RID: 13605
	public class CellSearchPattern : IExposable
	{
		// Token: 0x06013184 RID: 78212 RVA: 0x005A9DE0 File Offset: 0x005A7FE0
		public virtual void AddCellsToList(List<IntVec3> orderedCells, Thing thing, CellRect rect, IntVec3 focus, Rot4 focusRotation)
		{
			orderedCells.AddRange(rect.AdjacentCells);
		}

		// Token: 0x06013185 RID: 78213 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void ExposeData()
		{
		}
	}
}
