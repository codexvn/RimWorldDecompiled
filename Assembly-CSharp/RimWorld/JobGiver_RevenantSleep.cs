using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020014C4 RID: 5316
	public class JobGiver_RevenantSleep : ThinkNode_JobGiver
	{
		// Token: 0x06007F3E RID: 32574 RVA: 0x00265240 File Offset: 0x00263440
		protected override Job TryGiveJob(Pawn pawn)
		{
			return JobMaker.MakeJob(JobDefOf.RevenantSleep);
		}
	}
}
