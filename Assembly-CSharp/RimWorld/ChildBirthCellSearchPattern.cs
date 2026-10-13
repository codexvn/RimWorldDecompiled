using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02003528 RID: 13608
	public class ChildBirthCellSearchPattern : BedCellSearchPattern
	{
		// Token: 0x0601318E RID: 78222 RVA: 0x005AA060 File Offset: 0x005A8260
		public static void BedCellOffsets2xN(List<IntVec3> offsets, bool rightEdge, bool leftEdge)
		{
			offsets.Add(2 * IntVec3.South);
			if (rightEdge)
			{
				offsets.Add(IntVec3.West + IntVec3.South);
			}
			if (leftEdge)
			{
				offsets.Add(IntVec3.East + IntVec3.South);
			}
			offsets.Add(2 * IntVec3.South + IntVec3.West);
			offsets.Add(2 * IntVec3.South + IntVec3.East);
			if (rightEdge)
			{
				offsets.Add(IntVec3.West);
			}
			if (leftEdge)
			{
				offsets.Add(IntVec3.East);
			}
			offsets.Add(IntVec3.North);
			offsets.Add(IntVec3.North + IntVec3.West);
			offsets.Add(IntVec3.North + IntVec3.East);
			offsets.Add(IntVec3.South);
			offsets.Add(IntVec3.Zero);
		}

		// Token: 0x0601318F RID: 78223 RVA: 0x005AA14C File Offset: 0x005A834C
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
				ChildBirthCellSearchPattern.BedCellOffsets2xN(offsets, rightEdge, leftEdge);
				return;
			}
			throw new NotImplementedException("No offsets defined for bed of size {size}");
		}
	}
}
