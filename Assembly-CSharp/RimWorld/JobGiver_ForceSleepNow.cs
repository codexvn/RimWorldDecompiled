using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001459 RID: 5209
	public class JobGiver_ForceSleepNow : ThinkNode_JobGiver
	{
		// Token: 0x06007DF1 RID: 32241 RVA: 0x0025ECC3 File Offset: 0x0025CEC3
		protected override Job TryGiveJob(Pawn pawn)
		{
			Job job = JobMaker.MakeJob(JobDefOf.LayDown, pawn.Position);
			job.forceSleep = true;
			return job;
		}
	}
}
