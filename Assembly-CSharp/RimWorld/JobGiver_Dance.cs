using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001431 RID: 5169
	public class JobGiver_Dance : ThinkNode_JobGiver
	{
		// Token: 0x06007D85 RID: 32133 RVA: 0x0025CC3E File Offset: 0x0025AE3E
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_Dance jobGiver_Dance = (JobGiver_Dance)base.DeepCopy(resolve);
			jobGiver_Dance.ticksRange = this.ticksRange;
			return jobGiver_Dance;
		}

		// Token: 0x06007D86 RID: 32134 RVA: 0x0025CC58 File Offset: 0x0025AE58
		protected override Job TryGiveJob(Pawn pawn)
		{
			Job job = JobMaker.MakeJob(JobDefOf.Dance);
			job.expiryInterval = this.ticksRange.RandomInRange;
			return job;
		}

		// Token: 0x04005457 RID: 21591
		public IntRange ticksRange = new IntRange(300, 600);
	}
}
