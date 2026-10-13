using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020017C9 RID: 6089
	public class ThinkNode_ConditionalTargetPawnNotInBed : ThinkNode_Conditional
	{
		// Token: 0x06008DE4 RID: 36324 RVA: 0x002A174C File Offset: 0x0029F94C
		protected override bool Satisfied(Pawn pawn)
		{
			Pawn pawn2 = pawn.mindState.duty.focusSecond.Pawn;
			Building_Bed building_Bed = pawn.mindState.duty.focusThird.Thing as Building_Bed;
			if (building_Bed != null)
			{
				return pawn2.CurrentBed() != building_Bed;
			}
			return !pawn2.InBed();
		}
	}
}
