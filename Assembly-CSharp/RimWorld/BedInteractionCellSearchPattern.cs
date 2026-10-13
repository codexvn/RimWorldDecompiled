using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02003527 RID: 13607
	public class BedInteractionCellSearchPattern : BedCellSearchPattern
	{
		// Token: 0x0601318A RID: 78218 RVA: 0x005A9E80 File Offset: 0x005A8080
		public static void BedCellOffsets1x1(List<IntVec3> offsets)
		{
			offsets.Add(IntVec3.West);
			offsets.Add(IntVec3.East);
			offsets.Add(IntVec3.South);
			offsets.Add(IntVec3.North);
			offsets.Add(IntVec3.South + IntVec3.West);
			offsets.Add(IntVec3.South + IntVec3.East);
			offsets.Add(IntVec3.North + IntVec3.West);
			offsets.Add(IntVec3.North + IntVec3.East);
			offsets.Add(IntVec3.Zero);
		}

		// Token: 0x0601318B RID: 78219 RVA: 0x005A9F18 File Offset: 0x005A8118
		public static void BedCellOffsets2xN(List<IntVec3> offsets, bool rightEdge, bool leftEdge)
		{
			if (rightEdge)
			{
				offsets.Add(IntVec3.West);
			}
			if (leftEdge)
			{
				offsets.Add(IntVec3.East);
			}
			if (rightEdge)
			{
				offsets.Add(IntVec3.West + IntVec3.South);
			}
			if (leftEdge)
			{
				offsets.Add(IntVec3.East + IntVec3.South);
			}
			offsets.Add(IntVec3.North);
			offsets.Add(IntVec3.North + IntVec3.West);
			offsets.Add(IntVec3.North + IntVec3.East);
			offsets.Add(2 * IntVec3.South);
			offsets.Add(2 * IntVec3.South + IntVec3.West);
			offsets.Add(2 * IntVec3.South + IntVec3.East);
			offsets.Add(IntVec3.South);
			offsets.Add(IntVec3.Zero);
		}

		// Token: 0x0601318C RID: 78220 RVA: 0x005AA004 File Offset: 0x005A8204
		public override void BedCellOffsets(List<IntVec3> offsets, IntVec2 size, int slot)
		{
			if (size.z == 1 && size.x == 1)
			{
				BedInteractionCellSearchPattern.BedCellOffsets1x1(offsets);
				return;
			}
			if (size.z == 2)
			{
				bool rightEdge = slot == 0;
				bool leftEdge = slot == BedUtility.GetSleepingSlotsCount(size) - 1;
				BedInteractionCellSearchPattern.BedCellOffsets2xN(offsets, rightEdge, leftEdge);
				return;
			}
			throw new NotImplementedException("No offsets defined for bed of size {size}");
		}
	}
}
