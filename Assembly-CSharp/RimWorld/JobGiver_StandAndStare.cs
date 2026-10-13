using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001467 RID: 5223
	public class JobGiver_StandAndStare : ThinkNode_JobGiver
	{
		// Token: 0x06007E14 RID: 32276 RVA: 0x0025F57D File Offset: 0x0025D77D
		public override ThinkNode DeepCopy(bool resolve)
		{
			JobGiver_StandAndStare jobGiver_StandAndStare = (JobGiver_StandAndStare)base.DeepCopy(resolve);
			jobGiver_StandAndStare.locomotionUrgency = this.locomotionUrgency;
			jobGiver_StandAndStare.maxDanger = this.maxDanger;
			return jobGiver_StandAndStare;
		}

		// Token: 0x06007E15 RID: 32277 RVA: 0x0025F5A4 File Offset: 0x0025D7A4
		protected override Job TryGiveJob(Pawn pawn)
		{
			Pawn_MindState mindState = pawn.mindState;
			LocalTargetInfo? localTargetInfo;
			if (mindState == null)
			{
				localTargetInfo = null;
			}
			else
			{
				PawnDuty duty = mindState.duty;
				localTargetInfo = ((duty != null) ? new LocalTargetInfo?(duty.focusSecond) : null);
			}
			LocalTargetInfo? localTargetInfo2 = localTargetInfo;
			if (localTargetInfo2 != null)
			{
				LocalTargetInfo localTargetInfo3 = localTargetInfo2.GetValueOrDefault().Cell;
				if (pawn.Position != localTargetInfo3.Cell)
				{
					return null;
				}
			}
			if (pawn.Downed)
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.StandAndStare);
			job.locomotionUrgency = PawnUtility.ResolveLocomotion(pawn, this.locomotionUrgency);
			return job;
		}

		// Token: 0x040054A0 RID: 21664
		protected LocomotionUrgency locomotionUrgency = LocomotionUrgency.Walk;

		// Token: 0x040054A1 RID: 21665
		protected Danger maxDanger = Danger.Some;
	}
}
