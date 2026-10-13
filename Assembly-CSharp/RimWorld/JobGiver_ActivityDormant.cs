using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001477 RID: 5239
	public class JobGiver_ActivityDormant : ThinkNode_JobGiver
	{
		// Token: 0x06007E47 RID: 32327 RVA: 0x002604F6 File Offset: 0x0025E6F6
		protected override Job TryGiveJob(Pawn pawn)
		{
			Job job = JobMaker.MakeJob(JobDefOf.ActivityDormant);
			job.forceSleep = true;
			return job;
		}
	}
}
