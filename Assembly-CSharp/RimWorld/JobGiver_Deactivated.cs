using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200147E RID: 5246
	public class JobGiver_Deactivated : ThinkNode_JobGiver
	{
		// Token: 0x06007E5D RID: 32349 RVA: 0x00260899 File Offset: 0x0025EA99
		protected override Job TryGiveJob(Pawn pawn)
		{
			Job job = JobMaker.MakeJob(JobDefOf.Deactivated, pawn.Position);
			job.forceSleep = true;
			return job;
		}
	}
}
