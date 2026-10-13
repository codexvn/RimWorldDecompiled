using System;

namespace Verse.AI
{
	// Token: 0x02001015 RID: 4117
	public class ThinkNode_QueuedJob : ThinkNode
	{
		// Token: 0x060064EA RID: 25834 RVA: 0x001FEA8F File Offset: 0x001FCC8F
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_QueuedJob thinkNode_QueuedJob = (ThinkNode_QueuedJob)base.DeepCopy(resolve);
			thinkNode_QueuedJob.inBedOnly = this.inBedOnly;
			return thinkNode_QueuedJob;
		}

		// Token: 0x060064EB RID: 25835 RVA: 0x001FEAAC File Offset: 0x001FCCAC
		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			JobQueue jobQueue = pawn.jobs.jobQueue;
			if (pawn.Downed || jobQueue.AnyCanBeginNow(pawn, this.inBedOnly))
			{
				while (jobQueue.Count > 0 && !jobQueue.Peek().job.CanBeginNow(pawn, this.inBedOnly))
				{
					QueuedJob queuedJob = jobQueue.Dequeue();
					if (pawn.jobs.debugLog)
					{
						Pawn_JobTracker jobs = pawn.jobs;
						string str = "   Throwing away queued job that I cannot begin now: ";
						Job job = queuedJob.job;
						jobs.DebugLogEvent(str + ((job != null) ? job.ToString() : null));
					}
					queuedJob.Cleanup(pawn, false);
				}
			}
			if (!jobParams.ignoreQueue && jobQueue.Count > 0 && jobQueue.Peek().job.CanBeginNow(pawn, this.inBedOnly))
			{
				QueuedJob queuedJob2 = jobQueue.Dequeue();
				if (pawn.jobs.debugLog)
				{
					Pawn_JobTracker jobs2 = pawn.jobs;
					string str2 = "   Returning queued job: ";
					Job job2 = queuedJob2.job;
					jobs2.DebugLogEvent(str2 + ((job2 != null) ? job2.ToString() : null));
				}
				return new ThinkResult(queuedJob2.job, this, queuedJob2.tag, true);
			}
			return ThinkResult.NoJob;
		}

		// Token: 0x04004819 RID: 18457
		public bool inBedOnly;
	}
}
