using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200144C RID: 5196
	public class JobGiver_PrepareSkylantern : JobGiver_GotoAndStandSociallyActive
	{
		// Token: 0x06007DCE RID: 32206 RVA: 0x0025E3FC File Offset: 0x0025C5FC
		protected override Job TryGiveJob(Pawn pawn)
		{
			IntVec3 dest = base.GetDest(pawn);
			Job job = JobMaker.MakeJob(JobDefOf.PrepareSkylantern, dest);
			job.locomotionUrgency = this.locomotionUrgency;
			job.expiryInterval = this.expiryInterval;
			job.checkOverrideOnExpire = true;
			job.thingDefToCarry = this.def;
			job.count = this.count;
			return job;
		}

		// Token: 0x06007DCF RID: 32207 RVA: 0x0025E458 File Offset: 0x0025C658
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_PrepareSkylantern jobGiver_PrepareSkylantern = (JobGiver_PrepareSkylantern)base.DeepCopy(resolve);
			jobGiver_PrepareSkylantern.def = this.def;
			jobGiver_PrepareSkylantern.count = this.count;
			return jobGiver_PrepareSkylantern;
		}

		// Token: 0x0400547F RID: 21631
		public ThingDef def;

		// Token: 0x04005480 RID: 21632
		public int count = 1;
	}
}
