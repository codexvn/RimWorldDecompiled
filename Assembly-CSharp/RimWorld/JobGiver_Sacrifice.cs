using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200144D RID: 5197
	public class JobGiver_Sacrifice : ThinkNode_JobGiver
	{
		// Token: 0x06007DD1 RID: 32209 RVA: 0x0025E490 File Offset: 0x0025C690
		protected override Job TryGiveJob(Pawn pawn)
		{
			Pawn pawn2 = pawn.mindState.duty.focusSecond.Pawn;
			if (!pawn.CanReserveAndReach(pawn2, PathEndMode.ClosestTouch, Danger.None, 1, -1, null, false))
			{
				return null;
			}
			return JobMaker.MakeJob(JobDefOf.Sacrifice, pawn2, pawn.mindState.duty.focus);
		}
	}
}
