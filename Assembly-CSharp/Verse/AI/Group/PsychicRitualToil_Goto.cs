using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse.AI.Group
{
	// Token: 0x02001071 RID: 4209
	public class PsychicRitualToil_Goto : PsychicRitualToil
	{
		// Token: 0x17000FFA RID: 4090
		// (get) Token: 0x060066E5 RID: 26341 RVA: 0x00208982 File Offset: 0x00206B82
		public bool AnyPawnsArrived
		{
			get
			{
				return this.finalGoto.AnyPawnsDone;
			}
		}

		// Token: 0x17000FFB RID: 4091
		// (get) Token: 0x060066E6 RID: 26342 RVA: 0x0020898F File Offset: 0x00206B8F
		public bool FinalGatherPhase
		{
			get
			{
				return !this.allowWanderWhileWaiting || this.initialGoto.AllPawnsDone;
			}
		}

		// Token: 0x17000FFC RID: 4092
		// (get) Token: 0x060066E7 RID: 26343 RVA: 0x002089A6 File Offset: 0x00206BA6
		public virtual IEnumerable<Pawn> BlockingPawns
		{
			get
			{
				if (this.FinalGatherPhase)
				{
					return this.finalGoto.BlockingPawns;
				}
				return this.initialGoto.BlockingPawns;
			}
		}

		// Token: 0x060066E8 RID: 26344 RVA: 0x0020737D File Offset: 0x0020557D
		protected PsychicRitualToil_Goto()
		{
		}

		// Token: 0x060066E9 RID: 26345 RVA: 0x002089C8 File Offset: 0x00206BC8
		public PsychicRitualToil_Goto(IReadOnlyDictionary<PsychicRitualRoleDef, List<IntVec3>> rolePositions)
		{
			this.rolePositions = new Dictionary<PsychicRitualRoleDef, List<IntVec3>>(rolePositions.Count);
			foreach (KeyValuePair<PsychicRitualRoleDef, List<IntVec3>> keyValuePair in rolePositions)
			{
				PsychicRitualRoleDef psychicRitualRoleDef;
				List<IntVec3> list;
				keyValuePair.Deconstruct(out psychicRitualRoleDef, out list);
				PsychicRitualRoleDef key = psychicRitualRoleDef;
				List<IntVec3> collection = list;
				this.rolePositions[key] = new List<IntVec3>(collection);
			}
		}

		// Token: 0x060066EA RID: 26346 RVA: 0x00208A44 File Offset: 0x00206C44
		public virtual List<Pawn> ControlledPawns(PsychicRitual psychicRitual)
		{
			PsychicRitualToil_Goto.tmpControlledPawns.Clear();
			foreach (PsychicRitualRoleDef psychicRitualRoleDef in this.rolePositions.Keys)
			{
				if (psychicRitualRoleDef.psychicRitualWaitsForArrival)
				{
					PsychicRitualToil_Goto.tmpControlledPawns.AddRange(psychicRitual.assignments.AssignedPawns(psychicRitualRoleDef));
				}
			}
			return PsychicRitualToil_Goto.tmpControlledPawns;
		}

		// Token: 0x060066EB RID: 26347 RVA: 0x00208AC4 File Offset: 0x00206CC4
		public virtual bool WaitingOnPawn(Pawn pawn)
		{
			if (this.FinalGatherPhase)
			{
				return this.finalGoto.WaitingOnPawn(pawn);
			}
			return this.initialGoto.WaitingOnPawn(pawn);
		}

		// Token: 0x060066EC RID: 26348 RVA: 0x00208AE8 File Offset: 0x00206CE8
		public override void Start(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			List<Pawn> pawns = this.ControlledPawns(psychicRitual);
			this.initialGoto = new JobSyncTracker(pawns, this, "initial", "goto", JobCondition.Succeeded);
			this.finalGoto = new JobSyncTracker(pawns, this, "final", "goto", JobCondition.Succeeded);
			this.finalGatherPhaseStarted = !this.allowWanderWhileWaiting;
		}

		// Token: 0x060066ED RID: 26349 RVA: 0x00208B3C File Offset: 0x00206D3C
		public override void UpdateAllDuties(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			bool finalGatherPhase = this.FinalGatherPhase;
			foreach (KeyValuePair<PsychicRitualRoleDef, List<IntVec3>> keyValuePair in this.rolePositions)
			{
				PsychicRitualRoleDef psychicRitualRoleDef;
				List<IntVec3> list;
				keyValuePair.Deconstruct(out psychicRitualRoleDef, out list);
				PsychicRitualRoleDef role = psychicRitualRoleDef;
				List<IntVec3> list2 = list;
				int num = 0;
				foreach (Pawn pawn in psychicRitual.assignments.AssignedPawns(role))
				{
					IntVec3 c = list2[num++];
					DutyDef dutyDef;
					if (finalGatherPhase || this.initialGoto.WaitingOnPawn(pawn))
					{
						dutyDef = (pawn.IsPrisonerOfColony ? DutyDefOf.Idle : DutyDefOf.Goto);
					}
					else
					{
						dutyDef = DutyDefOf.WanderClose;
					}
					string text = finalGatherPhase ? "final" : "initial";
					Pawn pawn2 = pawn;
					DutyDef def = dutyDef;
					LocalTargetInfo? focus = new LocalTargetInfo?(c);
					string tag = text;
					this.SetPawnDuty(pawn2, psychicRitual, parent, def, focus, null, null, tag, 0f, null);
				}
			}
		}

		// Token: 0x060066EE RID: 26350 RVA: 0x00208C84 File Offset: 0x00206E84
		public override bool Tick(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			foreach (Pawn pawn in this.ControlledPawns(psychicRitual))
			{
				Pawn_MindState mindState = pawn.mindState;
				if (((mindState != null) ? mindState.duty : null) == null)
				{
					int num;
					if (this.lastUpdateDutyTick != GenTicks.TicksGame - 1)
					{
						num = 0;
					}
					else
					{
						int num2 = this.updatesInRow;
						this.updatesInRow = num2 + 1;
						num = num2;
					}
					this.updatesInRow = num;
					this.lastUpdateDutyTick = GenTicks.TicksGame;
					if (this.updatesInRow == 10)
					{
						Debug.LogWarning("UpdateAllDuties called 10 times in 10 ticks, ensure controlled pawns are being assigned valid duties.");
					}
					this.UpdateAllDuties(psychicRitual, parent);
					break;
				}
			}
			foreach (Pawn pawn2 in this.ControlledPawns(psychicRitual))
			{
				if (pawn2.IsPrisonerOfColony)
				{
					IntVec3 position = pawn2.Position;
					Pawn_MindState mindState2 = pawn2.mindState;
					IntVec3? intVec;
					if (mindState2 == null)
					{
						intVec = null;
					}
					else
					{
						PawnDuty duty = mindState2.duty;
						intVec = ((duty != null) ? new IntVec3?(duty.focus.Cell) : null);
					}
					if (position == intVec)
					{
						this.ForceFinished(pawn2);
					}
				}
				if (pawn2.Downed && pawn2.Spawned)
				{
					IntVec3 position = pawn2.Position;
					Pawn_MindState mindState3 = pawn2.mindState;
					IntVec3? intVec2;
					if (mindState3 == null)
					{
						intVec2 = null;
					}
					else
					{
						PawnDuty duty2 = mindState3.duty;
						intVec2 = ((duty2 != null) ? new IntVec3?(duty2.focus.Cell) : null);
					}
					if (!(position != intVec2))
					{
						this.ForceFinished(pawn2);
					}
				}
			}
			if (this.FinalGatherPhase && !this.finalGatherPhaseStarted)
			{
				this.finalGatherPhaseStarted = true;
				this.UpdateAllDuties(psychicRitual, parent);
				foreach (Pawn pawn3 in this.ControlledPawns(psychicRitual))
				{
					pawn3.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
				}
			}
			return this.finalGoto.AllPawnsDone;
		}

		// Token: 0x060066EF RID: 26351 RVA: 0x00208ECC File Offset: 0x002070CC
		private void ForceFinished(Pawn pawn)
		{
			if (this.FinalGatherPhase)
			{
				this.finalGoto.ForcePawnFinished(pawn);
				return;
			}
			this.initialGoto.ForcePawnFinished(pawn);
		}

		// Token: 0x060066F0 RID: 26352 RVA: 0x00208EF0 File Offset: 0x002070F0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.finalGatherPhaseStarted, "finalGatherPhaseStarted", false, false);
			Scribe_Deep.Look<JobSyncTracker>(ref this.initialGoto, "initialGoto", Array.Empty<object>());
			Scribe_Deep.Look<JobSyncTracker>(ref this.finalGoto, "finalGoto", Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.allowWanderWhileWaiting, "allowWanderWhileWaiting", false, false);
			Scribe_Collections.Look<PsychicRitualRoleDef, List<IntVec3>>(ref this.rolePositions, "rolePositions", LookMode.Def, LookMode.Value);
		}

		// Token: 0x060066F1 RID: 26353 RVA: 0x00208F64 File Offset: 0x00207164
		public override void Notify_PawnJobDone(PsychicRitual psychicRitual, PsychicRitualGraph parent, Pawn pawn, Job job, JobCondition condition)
		{
			this.initialGoto.Notify_PawnJobDone(pawn, job, condition);
			this.finalGoto.Notify_PawnJobDone(pawn, job, condition);
			LocalTargetInfo value = (LocalTargetInfo)psychicRitual.assignments.Target;
			if (this.FinalGatherPhase)
			{
				if (!this.finalGoto.WaitingOnPawn(pawn))
				{
					this.SetPawnDuty(pawn, psychicRitual, parent, DutyDefOf.WaitForRitualParticipants, new LocalTargetInfo?(value), new LocalTargetInfo?(pawn.PositionHeld), null, null, 0f, null);
					return;
				}
			}
			else if (!this.initialGoto.WaitingOnPawn(pawn))
			{
				this.SetPawnDuty(pawn, psychicRitual, parent, DutyDefOf.WaitForRitualParticipants, new LocalTargetInfo?(value), new LocalTargetInfo?(pawn.PositionHeld), null, null, 0f, null);
			}
		}

		// Token: 0x060066F2 RID: 26354 RVA: 0x00209042 File Offset: 0x00207242
		public override void Notify_PawnLost(PsychicRitual psychicRitual, PsychicRitualGraph parent, Pawn pawn, PawnLostCondition cond)
		{
			this.initialGoto.Notify_PawnLost(pawn);
			this.finalGoto.Notify_PawnLost(pawn);
		}

		// Token: 0x060066F3 RID: 26355 RVA: 0x0020905C File Offset: 0x0020725C
		public override ThinkResult Notify_DutyResult(PsychicRitual psychicRitual, PsychicRitualGraph parent, ThinkResult result, Pawn pawn, JobIssueParams issueParams)
		{
			if (result.Job != null)
			{
				return result;
			}
			PsychicRitualRoleDef psychicRitualRoleDef = psychicRitual.assignments.RoleForPawn(pawn, true);
			if (psychicRitualRoleDef == null)
			{
				return result;
			}
			if (psychicRitualRoleDef.ConditionAllowed(PsychicRitualRoleDef.Condition.NoPath))
			{
				return result;
			}
			if (psychicRitualRoleDef.CanReach(pawn, psychicRitual.assignments.Target))
			{
				return result;
			}
			TaggedString reason = PsychicRitualRoleDef.ConditionToReason(pawn, PsychicRitualRoleDef.Condition.NoPath);
			psychicRitual.LeaveOrCancelPsychicRitual(psychicRitualRoleDef, pawn, reason);
			return result;
		}

		// Token: 0x060066F4 RID: 26356 RVA: 0x002090C8 File Offset: 0x002072C8
		public override string GetJobReport(PsychicRitual psychicRitual, PsychicRitualGraph parent, Pawn pawn)
		{
			bool finalGatherPhase = this.FinalGatherPhase;
			if (!finalGatherPhase && this.initialGoto.WaitingOnPawn(pawn))
			{
				return "PsychicRitualToil_Goto_JobReport_Moving".Translate();
			}
			if (finalGatherPhase && this.finalGoto.WaitingOnPawn(pawn))
			{
				return "PsychicRitualToil_Goto_JobReport_Moving".Translate();
			}
			return "PsychicRitualToil_Goto_JobReport_Waiting".Translate();
		}

		// Token: 0x04004965 RID: 18789
		protected const string FinalSyncTag = "final";

		// Token: 0x04004966 RID: 18790
		protected const string InitialSyncTag = "initial";

		// Token: 0x04004967 RID: 18791
		protected const string GotoTag = "goto";

		// Token: 0x04004968 RID: 18792
		public bool allowWanderWhileWaiting;

		// Token: 0x04004969 RID: 18793
		protected Dictionary<PsychicRitualRoleDef, List<IntVec3>> rolePositions;

		// Token: 0x0400496A RID: 18794
		protected JobSyncTracker initialGoto;

		// Token: 0x0400496B RID: 18795
		protected JobSyncTracker finalGoto;

		// Token: 0x0400496C RID: 18796
		private bool finalGatherPhaseStarted;

		// Token: 0x0400496D RID: 18797
		private int lastUpdateDutyTick;

		// Token: 0x0400496E RID: 18798
		private int updatesInRow;

		// Token: 0x0400496F RID: 18799
		private static List<Pawn> tmpControlledPawns = new List<Pawn>(8);
	}
}
