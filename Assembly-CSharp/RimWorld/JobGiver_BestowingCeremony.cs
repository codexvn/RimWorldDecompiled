using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200147B RID: 5243
	public class JobGiver_BestowingCeremony : ThinkNode_JobGiver
	{
		// Token: 0x06007E57 RID: 32343 RVA: 0x00260780 File Offset: 0x0025E980
		protected override Job TryGiveJob(Pawn pawn)
		{
			PawnDuty duty = pawn.mindState.duty;
			if (duty == null)
			{
				return null;
			}
			return JobMaker.MakeJob(JobDefOf.BestowingCeremony, duty.focus.Pawn, duty.focusSecond);
		}
	}
}
