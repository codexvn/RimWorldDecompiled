using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x0200100B RID: 4107
	public class JobGiver_IdleError : ThinkNode_JobGiver
	{
		// Token: 0x060064B0 RID: 25776 RVA: 0x001FE1CD File Offset: 0x001FC3CD
		protected override Job TryGiveJob(Pawn pawn)
		{
			Log.ErrorOnce(((pawn != null) ? pawn.ToString() : null) + " issued IdleError wait job. The behavior tree should never get here.", 532983);
			Job job = JobMaker.MakeJob(JobDefOf.Wait);
			job.expiryInterval = 100;
			return job;
		}

		// Token: 0x040047FC RID: 18428
		private const int WaitTime = 100;
	}
}
