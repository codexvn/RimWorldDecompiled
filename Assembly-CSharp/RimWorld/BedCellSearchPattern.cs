using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02003526 RID: 13606
	public abstract class BedCellSearchPattern : CellSearchPattern
	{
		// Token: 0x06013187 RID: 78215
		public abstract void BedCellOffsets(List<IntVec3> offsets, IntVec2 size, int slot);

		// Token: 0x06013188 RID: 78216 RVA: 0x005A9DF0 File Offset: 0x005A7FF0
		public override void AddCellsToList(List<IntVec3> orderedCells, Thing thing, CellRect rect, IntVec3 focus, Rot4 focusRotation)
		{
			if (!rect.Contains(focus))
			{
				throw new ArgumentException();
			}
			int slotFromPosition = BedUtility.GetSlotFromPosition(focus, thing.Position, focusRotation, thing.def.size);
			this.BedCellOffsets(orderedCells, thing.def.size, slotFromPosition);
			RotationDirection relativeRotation = Rot4.GetRelativeRotation(Rot4.South, focusRotation);
			for (int i = 0; i < orderedCells.Count; i++)
			{
				orderedCells[i] = focus + orderedCells[i].RotatedBy(relativeRotation);
			}
		}
	}
}
