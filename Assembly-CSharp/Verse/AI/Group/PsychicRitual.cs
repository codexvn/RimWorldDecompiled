using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse.AI.Group
{
	// Token: 0x02001055 RID: 4181
	public class PsychicRitual : IExposable, ILoadReferenceable
	{
		// Token: 0x17000FE6 RID: 4070
		// (get) Token: 0x06006606 RID: 26118 RVA: 0x00205083 File Offset: 0x00203283
		public Map Map
		{
			get
			{
				return this.lord.Map;
			}
		}

		// Token: 0x17000FE7 RID: 4071
		// (get) Token: 0x06006607 RID: 26119 RVA: 0x00205090 File Offset: 0x00203290
		public float PowerPercent
		{
			get
			{
				if (this.maxPower == null)
				{
					return Mathf.Clamp01(this.power);
				}
				return Mathf.Clamp01(this.power / this.maxPower.Value);
			}
		}

		// Token: 0x06006608 RID: 26120 RVA: 0x002050C2 File Offset: 0x002032C2
		public PsychicRitual()
		{
			this.loadID = Find.UniqueIDsManager.GetNextPsychicRitualID();
		}

		// Token: 0x06006609 RID: 26121 RVA: 0x002050E4 File Offset: 0x002032E4
		public virtual void Start()
		{
			this.power = 0f;
			foreach (KeyValuePair<PsychicRitualRoleDef, List<Pawn>> keyValuePair in this.assignments.RoleAssignments)
			{
				PsychicRitualRoleDef psychicRitualRoleDef;
				List<Pawn> list;
				keyValuePair.Deconstruct(out psychicRitualRoleDef, out list);
				PsychicRitualRoleDef psychicRitualRoleDef2 = psychicRitualRoleDef;
				foreach (Pawn pawn in list)
				{
					if (!pawn.Awake() && !psychicRitualRoleDef2.ConditionAllowed(PsychicRitualRoleDef.Condition.Sleeping))
					{
						if (!pawn.health.capacities.CanBeAwake)
						{
							Log.Error(string.Format("Attempting to wake {0} for the {1} psychic ritual, but they cannot be awake.", pawn.ToStringSafe<Pawn>(), this.def.LabelCap));
						}
						RestUtility.WakeUp(pawn, false);
					}
					if (pawn != null)
					{
						Pawn_JobTracker jobs = pawn.jobs;
						if (jobs != null)
						{
							jobs.EndCurrentJob(JobCondition.InterruptForced, false, true);
						}
					}
					if (pawn != null)
					{
						Pawn_JobTracker jobs2 = pawn.jobs;
						if (jobs2 != null)
						{
							jobs2.ClearQueuedJobs(true);
						}
					}
					if (pawn.Drafted && !psychicRitualRoleDef2.ConditionAllowed(PsychicRitualRoleDef.Condition.Drafted))
					{
						pawn.drafter.Drafted = false;
					}
				}
			}
		}

		// Token: 0x0600660A RID: 26122 RVA: 0x00205230 File Offset: 0x00203430
		public virtual PsychicRitual.LeftOrCanceled LeaveOrCancelPsychicRitual(PsychicRitualRoleDef role, Pawn pawn, TaggedString reason)
		{
			if (this.def.nonRequiredPawnsMayLeave && this.assignments.RoleAssignedCount(role) > role.MinCount)
			{
				this.LeavePsychicRitual(pawn, reason);
				return PsychicRitual.LeftOrCanceled.Left;
			}
			this.CancelPsychicRitual(reason);
			return PsychicRitual.LeftOrCanceled.Canceled;
		}

		// Token: 0x0600660B RID: 26123 RVA: 0x00205268 File Offset: 0x00203468
		public virtual void CancelPsychicRitual(TaggedString reason)
		{
			Lord lord = this.lord;
			if (((lord != null) ? lord.faction : null) != null && this.lord.faction == Faction.OfPlayer)
			{
				Find.PsychicRitualManager.ClearCooldown(this.def);
				Messages.Message(this.def.PsychicRitualCanceledMessage(reason).CapitalizeFirst().EndWithPeriod(), this.assignments.Target, MessageTypeDefOf.NeutralEvent, true);
			}
			LordToil_PsychicRitual lordToil_PsychicRitual = this.lord.CurLordToil as LordToil_PsychicRitual;
			if (lordToil_PsychicRitual == null || lordToil_PsychicRitual.RitualData.removeLordOnCancel)
			{
				this.lord.lordManager.RemoveLord(this.lord);
			}
			this.canceled = true;
		}

		// Token: 0x0600660C RID: 26124 RVA: 0x00205328 File Offset: 0x00203528
		public virtual void LeavePsychicRitual(Pawn pawn, TaggedString reason)
		{
			this.TryMessagePawnLost(pawn, reason);
			if (this.lord.ownedPawns.Contains(pawn))
			{
				this.lord.Notify_PawnLost(pawn, PawnLostCondition.LordRejected, null);
			}
		}

		// Token: 0x0600660D RID: 26125 RVA: 0x00205367 File Offset: 0x00203567
		public virtual void ReleaseAllPawnsAndBuildings()
		{
			this.lord.RemoveAllPawns(true);
			this.lord.RemoveAllBuildings();
		}

		// Token: 0x0600660E RID: 26126 RVA: 0x00205380 File Offset: 0x00203580
		public void TryMessagePawnLost(Pawn pawn, TaggedString reason)
		{
			Lord lord = this.lord;
			if (((lord != null) ? lord.faction : null) == Faction.OfPlayer && PawnUtility.ShouldSendNotificationAbout(pawn))
			{
				Messages.Message(this.def.LeftPsychicRitualMessage(pawn, reason).CapitalizeFirst().EndWithPeriod(), pawn, MessageTypeDefOf.NeutralEvent, false);
			}
		}

		// Token: 0x0600660F RID: 26127 RVA: 0x002053E4 File Offset: 0x002035E4
		public virtual void Notify_PawnLost(Pawn pawn, PawnLostCondition cond)
		{
			Pawn_JobTracker jobs = pawn.jobs;
			Lord lord;
			if (jobs == null)
			{
				lord = null;
			}
			else
			{
				Job curJob = jobs.curJob;
				lord = ((curJob != null) ? curJob.lord : null);
			}
			if (lord == this.lord)
			{
				Pawn_JobTracker jobs2 = pawn.jobs;
				if (jobs2 != null)
				{
					jobs2.EndCurrentJob(JobCondition.InterruptForced, true, true);
				}
			}
			if (cond == PawnLostCondition.LordRejected)
			{
				return;
			}
			if (this.succeeded)
			{
				return;
			}
			TaggedString reason = PsychicRitualRoleDef.PawnLostConditionToPsychicRitualReason(this, pawn, cond);
			PsychicRitualRoleDef psychicRitualRoleDef = this.assignments.RoleForPawn(pawn, true);
			if (psychicRitualRoleDef != null && this.assignments.RoleAssignedCount(psychicRitualRoleDef) <= psychicRitualRoleDef.MinCount)
			{
				this.CancelPsychicRitual(reason);
				return;
			}
			if (!this.def.nonRequiredPawnsMayLeave)
			{
				this.CancelPsychicRitual(reason);
				return;
			}
			if (this.lord.ownedPawns.Contains(pawn))
			{
				this.TryMessagePawnLost(pawn, reason);
			}
		}

		// Token: 0x06006610 RID: 26128 RVA: 0x002054A4 File Offset: 0x002036A4
		public virtual void ExposeData()
		{
			Scribe_References.Look<Lord>(ref this.lord, "lord", false);
			Scribe_Defs.Look<PsychicRitualDef>(ref this.def, "def");
			Scribe_Values.Look<float>(ref this.power, "power", 0f, false);
			Scribe_Values.Look<float?>(ref this.maxPower, "maxPower", null, false);
			Scribe_Values.Look<bool>(ref this.canceled, "canceled", false, false);
			Scribe_Values.Look<bool>(ref this.succeeded, "succeeded", false, false);
			if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs)
			{
				LordJob_PsychicRitual lordJob_PsychicRitual = this.lord.LordJob as LordJob_PsychicRitual;
				if (lordJob_PsychicRitual != null)
				{
					this.assignments = lordJob_PsychicRitual.assignments;
					return;
				}
				Log.Error("Error during loading psychic ritual: LordJob is not a LordJob_PsychicRitual.");
			}
		}

		// Token: 0x06006611 RID: 26129 RVA: 0x00205559 File Offset: 0x00203759
		public string GetUniqueLoadID()
		{
			return "PsychicRitual_" + this.loadID.ToString();
		}

		// Token: 0x040048FE RID: 18686
		public Lord lord;

		// Token: 0x040048FF RID: 18687
		public PsychicRitualDef def;

		// Token: 0x04004900 RID: 18688
		public PsychicRitualRoleAssignments assignments;

		// Token: 0x04004901 RID: 18689
		public bool canceled;

		// Token: 0x04004902 RID: 18690
		public bool succeeded;

		// Token: 0x04004903 RID: 18691
		private int loadID = -1;

		// Token: 0x04004904 RID: 18692
		public float power;

		// Token: 0x04004905 RID: 18693
		public float? maxPower;

		// Token: 0x02001056 RID: 4182
		public enum LeftOrCanceled
		{
			// Token: 0x04004907 RID: 18695
			Invalid,
			// Token: 0x04004908 RID: 18696
			Left,
			// Token: 0x04004909 RID: 18697
			Canceled
		}
	}
}
