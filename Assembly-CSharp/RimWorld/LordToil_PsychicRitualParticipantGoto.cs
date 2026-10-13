using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001712 RID: 5906
	public class LordToil_PsychicRitualParticipantGoto : LordToil
	{
		// Token: 0x17001561 RID: 5473
		// (get) Token: 0x06008B15 RID: 35605 RVA: 0x002972B9 File Offset: 0x002954B9
		protected LordToilData_PsychicRitualParticipantGoto Data
		{
			get
			{
				return (LordToilData_PsychicRitualParticipantGoto)this.data;
			}
		}

		// Token: 0x06008B16 RID: 35606 RVA: 0x002972C6 File Offset: 0x002954C6
		public LordToil_PsychicRitualParticipantGoto(int timeoutTicks = -1)
		{
			this.data = new LordToilData_PsychicRitualParticipantGoto();
			this.Data.timeoutTicks = timeoutTicks;
		}

		// Token: 0x17001562 RID: 5474
		// (get) Token: 0x06008B17 RID: 35607 RVA: 0x002972E5 File Offset: 0x002954E5
		public bool AnyPawnsArrived
		{
			get
			{
				return this.Data.finalGoto.AnyPawnsDone;
			}
		}

		// Token: 0x17001563 RID: 5475
		// (get) Token: 0x06008B18 RID: 35608 RVA: 0x002972F7 File Offset: 0x002954F7
		public bool AllPawnsArrived
		{
			get
			{
				return this.Data.finalGoto.AllPawnsDone;
			}
		}

		// Token: 0x17001564 RID: 5476
		// (get) Token: 0x06008B19 RID: 35609 RVA: 0x00297309 File Offset: 0x00295509
		public bool TimedOut
		{
			get
			{
				return this.Data.timeoutTicks != -1 && this.Data.ticksElapsed >= this.Data.timeoutTicks;
			}
		}

		// Token: 0x06008B1A RID: 35610 RVA: 0x00297336 File Offset: 0x00295536
		public LordToil_PsychicRitualParticipantGoto(IEnumerable<PsychicRitualParticipant> participants)
		{
			this.data = new LordToilData_PsychicRitualParticipantGoto();
			this.SetParticipants(participants);
		}

		// Token: 0x06008B1B RID: 35611 RVA: 0x00297350 File Offset: 0x00295550
		public override void UpdateAllDuties()
		{
			if (this.Data.participants.Count == 0)
			{
				return;
			}
			foreach (PsychicRitualParticipant psychicRitualParticipant in this.Participants)
			{
				Pawn pawn;
				IntVec3 intVec;
				psychicRitualParticipant.Deconstruct(out pawn, out intVec);
				Pawn pawn2 = pawn;
				IntVec3 c = intVec;
				DutyDef goto_NoZeroLengthPaths = DutyDefOf.Goto_NoZeroLengthPaths;
				pawn2.mindState.duty = new PawnDuty(goto_NoZeroLengthPaths, c, -1f)
				{
					tag = "final"
				};
			}
		}

		// Token: 0x06008B1C RID: 35612 RVA: 0x002973E4 File Offset: 0x002955E4
		public override void LordToilTick()
		{
			this.Data.ticksElapsed++;
		}

		// Token: 0x06008B1D RID: 35613 RVA: 0x002973FC File Offset: 0x002955FC
		public override void Notify_PawnJobDone(Pawn pawn, JobCondition condition)
		{
			LordToilData_PsychicRitualParticipantGoto data = this.Data;
			if (data != null)
			{
				JobSyncTracker finalGoto = data.finalGoto;
				if (finalGoto != null)
				{
					finalGoto.Notify_PawnJobDone(pawn, pawn.CurJob, condition);
				}
			}
			LordToilData_PsychicRitualParticipantGoto data2 = this.Data;
			if (((data2 != null) ? data2.finalGoto : null) != null && !this.Data.finalGoto.WaitingOnPawn(pawn))
			{
				Pawn_MindState mindState = pawn.mindState;
				if (((mindState != null) ? mindState.duty : null) != null)
				{
					pawn.mindState.duty.def = DutyDefOf.Idle;
				}
			}
		}

		// Token: 0x06008B1E RID: 35614 RVA: 0x0029747D File Offset: 0x0029567D
		public override void Notify_PawnLost(Pawn victim, PawnLostCondition cond)
		{
			this.Data.finalGoto.Notify_PawnLost(victim);
		}

		// Token: 0x06008B1F RID: 35615 RVA: 0x00297490 File Offset: 0x00295690
		public void SetParticipants(IEnumerable<PsychicRitualParticipant> participants)
		{
			PsychicRitualParticipant[] array = (participants as PsychicRitualParticipant[]) ?? participants.ToArray<PsychicRitualParticipant>();
			this.Data.participants = new List<PsychicRitualParticipant>(array);
			this.Data.finalGoto = new JobSyncTracker(from participant in array
			select participant.pawn, null, "final", null, JobCondition.Succeeded);
		}

		// Token: 0x17001565 RID: 5477
		// (get) Token: 0x06008B20 RID: 35616 RVA: 0x002974FC File Offset: 0x002956FC
		public virtual IEnumerable<PsychicRitualParticipant> Participants
		{
			get
			{
				return this.Data.participants;
			}
		}

		// Token: 0x04005A65 RID: 23141
		private const string FinalSyncTag = "final";
	}
}
