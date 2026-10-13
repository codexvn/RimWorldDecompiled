using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200151C RID: 5404
	public class LearningGiver_NatureRunning : LearningGiver
	{
		// Token: 0x060080B7 RID: 32951 RVA: 0x0026C73C File Offset: 0x0026A93C
		public override bool CanDo(Pawn pawn)
		{
			LocalTargetInfo localTargetInfo;
			return base.CanDo(pawn) && NatureRunningUtility.TryFindNatureInterestTarget(pawn, out localTargetInfo);
		}

		// Token: 0x060080B8 RID: 32952 RVA: 0x0026C75C File Offset: 0x0026A95C
		public override Job TryGiveJob(Pawn pawn)
		{
			LocalTargetInfo targetA;
			if (!NatureRunningUtility.TryFindNatureInterestTarget(pawn, out targetA))
			{
				return null;
			}
			Job job = JobMaker.MakeJob(this.def.jobDef, targetA);
			job.locomotionUrgency = LocomotionUrgency.Sprint;
			return job;
		}
	}
}
