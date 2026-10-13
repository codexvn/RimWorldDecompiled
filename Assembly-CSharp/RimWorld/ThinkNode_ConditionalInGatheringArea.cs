using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020017B6 RID: 6070
	public class ThinkNode_ConditionalInGatheringArea : ThinkNode_Conditional
	{
		// Token: 0x06008DBB RID: 36283 RVA: 0x002A11F0 File Offset: 0x0029F3F0
		protected override bool Satisfied(Pawn pawn)
		{
			if (pawn.mindState.duty == null)
			{
				return false;
			}
			IntVec3 cell = pawn.mindState.duty.focus.Cell;
			return GatheringsUtility.InGatheringArea(pawn.Position, cell, pawn.Map);
		}
	}
}
