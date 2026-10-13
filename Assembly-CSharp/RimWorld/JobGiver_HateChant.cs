using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001448 RID: 5192
	public class JobGiver_HateChant : ThinkNode_JobGiver
	{
		// Token: 0x06007DC5 RID: 32197 RVA: 0x0025E258 File Offset: 0x0025C458
		protected override Job TryGiveJob(Pawn pawn)
		{
			return JobMaker.MakeJob(JobDefOf.HateChanting);
		}
	}
}
