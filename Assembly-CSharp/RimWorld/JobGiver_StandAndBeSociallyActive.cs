using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001452 RID: 5202
	public class JobGiver_StandAndBeSociallyActive : ThinkNode_JobGiver
	{
		// Token: 0x06007DDC RID: 32220 RVA: 0x0025E817 File Offset: 0x0025CA17
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_StandAndBeSociallyActive jobGiver_StandAndBeSociallyActive = (JobGiver_StandAndBeSociallyActive)base.DeepCopy(resolve);
			jobGiver_StandAndBeSociallyActive.ticksRange = this.ticksRange;
			jobGiver_StandAndBeSociallyActive.lookDirection = this.lookDirection;
			jobGiver_StandAndBeSociallyActive.maintainFacing = this.maintainFacing;
			return jobGiver_StandAndBeSociallyActive;
		}

		// Token: 0x06007DDD RID: 32221 RVA: 0x0025E849 File Offset: 0x0025CA49
		protected override Job TryGiveJob(Pawn pawn)
		{
			Job job = JobMaker.MakeJob(JobDefOf.StandAndBeSociallyActive);
			job.expiryInterval = this.ticksRange.RandomInRange;
			job.lookDirection = this.lookDirection;
			job.forceMaintainFacing = this.maintainFacing;
			return job;
		}

		// Token: 0x04005484 RID: 21636
		public IntRange ticksRange = new IntRange(300, 600);

		// Token: 0x04005485 RID: 21637
		public Direction8Way lookDirection;

		// Token: 0x04005486 RID: 21638
		public bool maintainFacing;
	}
}
