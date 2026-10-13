using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002796 RID: 10134
	public class RitualPosition_PreferredAdjacentCellCardinal : RitualPosition_Cells
	{
		// Token: 0x0600DDFE RID: 56830 RVA: 0x004154F4 File Offset: 0x004136F4
		public override void FindCells(List<IntVec3> cells, Thing thing, CellRect rect, IntVec3 spot, Rot4 rotation)
		{
			if (this.preferredCellSearchPattern != null)
			{
				this.preferredCellSearchPattern.AddCellsToList(cells, thing, rect, spot, (thing != null) ? thing.Rotation : Rot4.South);
				return;
			}
			cells.AddRange(rect.AdjacentCellsCardinal);
		}

		// Token: 0x0600DDFF RID: 56831 RVA: 0x0041552C File Offset: 0x0041372C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<CellSearchPattern>(ref this.preferredCellSearchPattern, "preferredCellSearchPattern", Array.Empty<object>());
		}

		// Token: 0x040095AE RID: 38318
		public CellSearchPattern preferredCellSearchPattern;
	}
}
