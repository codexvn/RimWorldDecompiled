using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02001009 RID: 4105
	public class JobGiver_Idle : ThinkNode_JobGiver
	{
		// Token: 0x060064AB RID: 25771 RVA: 0x001FE0F8 File Offset: 0x001FC2F8
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_Idle jobGiver_Idle = (JobGiver_Idle)base.DeepCopy(resolve);
			jobGiver_Idle.ticks = this.ticks;
			return jobGiver_Idle;
		}

		// Token: 0x060064AC RID: 25772 RVA: 0x001FE114 File Offset: 0x001FC314
		protected override Job TryGiveJob(Pawn pawn)
		{
			Job job = JobMaker.MakeJob(JobDefOf.Wait);
			job.expiryInterval = this.ticks;
			Pawn_MindState mindState = pawn.mindState;
			Rot4? rot;
			if (mindState == null)
			{
				rot = null;
			}
			else
			{
				PawnDuty duty = mindState.duty;
				rot = ((duty != null) ? new Rot4?(duty.overrideFacing) : null);
			}
			job.overrideFacing = (rot ?? Rot4.Invalid);
			return job;
		}

		// Token: 0x040047FA RID: 18426
		public int ticks = 50;
	}
}
