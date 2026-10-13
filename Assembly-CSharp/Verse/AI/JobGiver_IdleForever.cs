using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x0200100A RID: 4106
	public class JobGiver_IdleForever : ThinkNode_JobGiver
	{
		// Token: 0x060064AE RID: 25774 RVA: 0x001FE198 File Offset: 0x001FC398
		protected override Job TryGiveJob(Pawn pawn)
		{
			Job job = JobMaker.MakeJob(JobDefOf.Wait_Downed);
			if (pawn.Deathresting)
			{
				job.forceSleep = true;
			}
			else
			{
				job.expiryInterval = 2500;
			}
			return job;
		}

		// Token: 0x040047FB RID: 18427
		private const int checkCrawlInterval = 2500;
	}
}
