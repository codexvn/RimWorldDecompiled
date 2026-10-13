using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200145F RID: 5215
	public class JobGiver_LayDownAwake : ThinkNode_JobGiver
	{
		// Token: 0x06007E01 RID: 32257 RVA: 0x0025F309 File Offset: 0x0025D509
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (!pawn.CanReach(pawn.mindState.duty.focusThird, PathEndMode.Touch, Danger.None, false, false, TraverseMode.ByPawn))
			{
				return null;
			}
			return JobMaker.MakeJob(JobDefOf.LayDownAwake, pawn.mindState.duty.focusThird);
		}
	}
}
