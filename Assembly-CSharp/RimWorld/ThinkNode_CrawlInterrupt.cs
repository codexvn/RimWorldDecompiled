using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020017D8 RID: 6104
	public class ThinkNode_CrawlInterrupt : ThinkNode_Priority
	{
		// Token: 0x06008E06 RID: 36358 RVA: 0x002A1B34 File Offset: 0x0029FD34
		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			pawn.mindState.nextMoveOrderIsWait = false;
			ThinkResult thinkResult = base.TryIssueJobPackage(pawn, jobParams);
			if (thinkResult == ThinkResult.NoJob)
			{
				return ThinkResult.NoJob;
			}
			if (pawn.mindState.nextMoveOrderIsCrawlBreak)
			{
				pawn.mindState.nextMoveOrderIsCrawlBreak = false;
				Job job = JobMaker.MakeJob(JobDefOf.Wait_Downed);
				job.expiryInterval = this.ticksBetweenCrawlsRange.RandomInRange;
				return new ThinkResult(job, this, null, false);
			}
			pawn.mindState.nextMoveOrderIsCrawlBreak = true;
			thinkResult.Job.expiryInterval = this.crawlDurationTicksRange.RandomInRange;
			return thinkResult;
		}

		// Token: 0x04005BE7 RID: 23527
		private IntRange crawlDurationTicksRange = new IntRange(720, 1440);

		// Token: 0x04005BE8 RID: 23528
		private IntRange ticksBetweenCrawlsRange = new IntRange(240, 480);
	}
}
