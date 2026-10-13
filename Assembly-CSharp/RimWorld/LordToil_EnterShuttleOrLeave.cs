using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020016F4 RID: 5876
	public class LordToil_EnterShuttleOrLeave : LordToil
	{
		// Token: 0x17001546 RID: 5446
		// (get) Token: 0x06008A94 RID: 35476 RVA: 0x00002501 File Offset: 0x00000701
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06008A95 RID: 35477 RVA: 0x002953ED File Offset: 0x002935ED
		public LordToil_EnterShuttleOrLeave(Thing shuttle, LocomotionUrgency locomotion = LocomotionUrgency.None, bool canDig = false, bool interruptCurrentJob = false)
		{
			this.shuttle = shuttle;
			this.locomotion = locomotion;
			this.canDig = canDig;
			this.interruptCurrentJob = interruptCurrentJob;
		}

		// Token: 0x06008A96 RID: 35478 RVA: 0x00295412 File Offset: 0x00293612
		private DutyDef GetExpectedDutyDef(Pawn pawn)
		{
			if (!this.shuttle.Spawned || !pawn.CanReach(this.shuttle, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				return DutyDefOf.ExitMapBestAndDefendSelf;
			}
			return DutyDefOf.EnterTransporterAndDefendSelf;
		}

		// Token: 0x06008A97 RID: 35479 RVA: 0x00295444 File Offset: 0x00293644
		private void EnsureCorrectDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				Pawn pawn = this.lord.ownedPawns[i];
				DutyDef expectedDutyDef = this.GetExpectedDutyDef(pawn);
				if (pawn.mindState != null && (pawn.mindState.duty == null || pawn.mindState.duty.def != expectedDutyDef))
				{
					pawn.mindState.duty = new PawnDuty(expectedDutyDef, this.shuttle, -1f);
					pawn.mindState.duty.locomotion = this.locomotion;
					pawn.mindState.duty.canDig = this.canDig;
					if (this.interruptCurrentJob && pawn.jobs != null && pawn.jobs.curJob != null)
					{
						pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
					}
				}
			}
		}

		// Token: 0x06008A98 RID: 35480 RVA: 0x00295530 File Offset: 0x00293730
		public override void UpdateAllDuties()
		{
			this.EnsureCorrectDuties();
		}

		// Token: 0x06008A99 RID: 35481 RVA: 0x00295530 File Offset: 0x00293730
		public override void LordToilTick()
		{
			this.EnsureCorrectDuties();
		}

		// Token: 0x04005A3D RID: 23101
		public Thing shuttle;

		// Token: 0x04005A3E RID: 23102
		public LocomotionUrgency locomotion;

		// Token: 0x04005A3F RID: 23103
		public bool canDig;

		// Token: 0x04005A40 RID: 23104
		public bool interruptCurrentJob;
	}
}
