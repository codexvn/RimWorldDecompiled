using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02001053 RID: 4179
	public class LordToil_PsychicRitual : LordToil
	{
		// Token: 0x17000FE3 RID: 4067
		// (get) Token: 0x060065EF RID: 26095 RVA: 0x002049E1 File Offset: 0x00202BE1
		public PsychicRitualLordToilData RitualData
		{
			get
			{
				return (PsychicRitualLordToilData)this.data;
			}
		}

		// Token: 0x060065F0 RID: 26096 RVA: 0x002049EE File Offset: 0x00202BEE
		public LordToil_PsychicRitual(PsychicRitualDef def, PsychicRitualRoleAssignments assignments)
		{
			this.def = def;
			this.assignments = assignments;
		}

		// Token: 0x060065F1 RID: 26097 RVA: 0x00204A0B File Offset: 0x00202C0B
		public LordToil_PsychicRitual(Func<PsychicRitualToil> psychicRitualToilGenerator)
		{
			this.psychicRitualToilGenerator = psychicRitualToilGenerator;
		}

		// Token: 0x060065F2 RID: 26098 RVA: 0x00204A24 File Offset: 0x00202C24
		protected virtual PsychicRitualToil GeneratePsychicRitualToil()
		{
			PsychicRitualToil psychicRitualToil;
			if (this.def != null)
			{
				psychicRitualToil = this.def.CreateGraph();
				if (psychicRitualToil == null)
				{
					throw new InvalidOperationException(string.Format("PsychicRitualDef {0} returned a null graph during CreateGraph().", this.def));
				}
			}
			else
			{
				Func<PsychicRitualToil> func = this.psychicRitualToilGenerator;
				psychicRitualToil = ((func != null) ? func() : null);
				if (psychicRitualToil == null)
				{
					throw new InvalidOperationException("PsychicRitualToil generator " + this.psychicRitualToilGenerator.ToStringSafe<Func<PsychicRitualToil>>() + " returned a null toil when invoked.");
				}
			}
			return psychicRitualToil;
		}

		// Token: 0x060065F3 RID: 26099 RVA: 0x00204A98 File Offset: 0x00202C98
		public override void Init()
		{
			this.data = new PsychicRitualLordToilData();
			this.RitualData.psychicRitual = new PsychicRitual
			{
				lord = this.lord,
				def = this.def,
				assignments = this.assignments
			};
			this.RitualData.iteration = this.iteration;
			this.RitualData.psychicRitualToil = this.GeneratePsychicRitualToil();
			this.RitualData.playerRitual = (this.RitualData.psychicRitual.lord.faction == Faction.OfPlayer);
			this.RitualData.removeLordOnCancel = this.removeLordOnCancel;
			float value;
			this.def.CalculateMaxPower(this.assignments, null, out value);
			this.RitualData.psychicRitual.maxPower = new float?(value);
			this.RitualData.psychicRitual.Start();
			this.RitualData.psychicRitualToil.Start(this.RitualData.psychicRitual, null);
			if (this.RitualData.playerRitual)
			{
				Messages.Message(this.def.PsychicRitualBegunMessage(this.assignments).CapitalizeFirst(), this.assignments.Target, MessageTypeDefOf.NeutralEvent, true);
			}
		}

		// Token: 0x060065F4 RID: 26100 RVA: 0x00204BDA File Offset: 0x00202DDA
		public override void UpdateAllDuties()
		{
			this.RitualData.psychicRitualToil.UpdateAllDuties(this.RitualData.psychicRitual, null);
		}

		// Token: 0x060065F5 RID: 26101 RVA: 0x00204BF8 File Offset: 0x00202DF8
		public override void LordToilTick()
		{
			if (this.RitualData.done)
			{
				return;
			}
			if (!this.RitualData.psychicRitual.succeeded)
			{
				this.RitualData.psychicRitual.def.RemoveIncapablePawns(this.RitualData.psychicRitual);
			}
			this.RitualData.psychicRitual.def.CheckPsychicRitualCancelConditions(this.RitualData.psychicRitual);
			if (this.RitualData.done)
			{
				return;
			}
			this.RitualData.done = this.RitualData.psychicRitualToil.Tick(this.RitualData.psychicRitual, null);
			if (this.RitualData.psychicRitual.canceled)
			{
				this.lord.ReceiveMemo("PsychicRitualCanceled");
				return;
			}
			if (this.RitualData.done)
			{
				this.RitualCompleted();
			}
		}

		// Token: 0x060065F6 RID: 26102 RVA: 0x00204CD0 File Offset: 0x00202ED0
		private void RitualCompleted()
		{
			if (this.RitualData.playerRitual)
			{
				Find.IdeoManager.lastPsychicRitualPerformedTick = Find.TickManager.TicksGame;
				Messages.Message(this.RitualData.psychicRitual.def.PsychicRitualCompletedMessage().CapitalizeFirst(), this.assignments.Target, MessageTypeDefOf.NeutralEvent, true);
			}
			Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.PsychicRitualPerformed), true);
			foreach (Pawn arg in this.assignments.AllAssignedPawns)
			{
				Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.InvolvedInPsychicRitual, arg.Named(HistoryEventArgsNames.Doer)), true);
			}
			this.lord.ReceiveMemo("PsychicRitualCompleted" + this.RitualData.iteration.ToString());
		}

		// Token: 0x060065F7 RID: 26103 RVA: 0x00204DD4 File Offset: 0x00202FD4
		public override void Cleanup()
		{
			this.RitualData.psychicRitualToil.End(this.RitualData.psychicRitual, null, false);
			this.RitualData.done = true;
		}

		// Token: 0x17000FE4 RID: 4068
		// (get) Token: 0x060065F8 RID: 26104 RVA: 0x00204DFF File Offset: 0x00202FFF
		public override bool ShouldFail
		{
			get
			{
				return this.RitualData.done || base.ShouldFail;
			}
		}

		// Token: 0x060065F9 RID: 26105 RVA: 0x00204E16 File Offset: 0x00203016
		public override IEnumerable<Gizmo> GetPawnGizmos(Pawn pawn)
		{
			return this.RitualData.psychicRitualToil.GetPawnGizmos(this.RitualData.psychicRitual, null, pawn);
		}

		// Token: 0x060065FA RID: 26106 RVA: 0x00204E35 File Offset: 0x00203035
		public override IEnumerable<Gizmo> GetBuildingGizmos(Building building)
		{
			return this.RitualData.psychicRitualToil.GetBuildingGizmos(this.RitualData.psychicRitual, null, building);
		}

		// Token: 0x060065FB RID: 26107 RVA: 0x00204E54 File Offset: 0x00203054
		public override void Notify_PawnLost(Pawn victim, PawnLostCondition condition)
		{
			PsychicRitual psychicRitual = this.RitualData.psychicRitual;
			psychicRitual.Notify_PawnLost(victim, condition);
			this.RitualData.psychicRitualToil.Notify_PawnLost(psychicRitual, null, victim, condition);
			psychicRitual.assignments.RemoveParticipant(victim);
		}

		// Token: 0x060065FC RID: 26108 RVA: 0x00204E98 File Offset: 0x00203098
		public override void Notify_PawnJobDone(Pawn pawn, JobCondition condition)
		{
			if (pawn.CurJob.lord != this.lord)
			{
				return;
			}
			this.RitualData.psychicRitualToil.Notify_PawnJobDone(this.RitualData.psychicRitual, null, pawn, pawn.CurJob, condition);
			PsychicRitualRoleAssignments psychicRitualRoleAssignments = this.RitualData.psychicRitual.assignments;
			PsychicRitualRoleDef psychicRitualRoleDef = (psychicRitualRoleAssignments != null) ? psychicRitualRoleAssignments.RoleForPawn(pawn, true) : null;
			if (psychicRitualRoleDef != null)
			{
				psychicRitualRoleDef.Notify_PawnJobDone(this.RitualData.psychicRitual, pawn, condition);
			}
		}

		// Token: 0x060065FD RID: 26109 RVA: 0x00204F12 File Offset: 0x00203112
		public override void Notify_BuildingLost(Building building)
		{
			this.RitualData.psychicRitualToil.Notify_BuildingLost(this.RitualData.psychicRitual, null, building);
		}

		// Token: 0x060065FE RID: 26110 RVA: 0x00204F31 File Offset: 0x00203131
		public override void Notify_CorpseLost(Corpse corpse)
		{
			this.RitualData.psychicRitualToil.Notify_CorpseLost(this.RitualData.psychicRitual, null, corpse);
		}

		// Token: 0x060065FF RID: 26111 RVA: 0x00204F50 File Offset: 0x00203150
		public override void Notify_ReachedDutyLocation(Pawn pawn)
		{
			this.RitualData.psychicRitualToil.Notify_ReachedDutyLocation(this.RitualData.psychicRitual, null, pawn);
		}

		// Token: 0x06006600 RID: 26112 RVA: 0x00204F6F File Offset: 0x0020316F
		public override void Notify_ConstructionFailed(Pawn pawn, Frame frame, Blueprint_Build newBlueprint)
		{
			this.RitualData.psychicRitualToil.Notify_ConstructionFailed(this.RitualData.psychicRitual, null, pawn, frame, newBlueprint);
		}

		// Token: 0x06006601 RID: 26113 RVA: 0x00204F90 File Offset: 0x00203190
		public override void Notify_BuildingSpawnedOnMap(Building building)
		{
			this.RitualData.psychicRitualToil.Notify_BuildingSpawnedOnMap(this.RitualData.psychicRitual, null, building);
		}

		// Token: 0x06006602 RID: 26114 RVA: 0x00204FAF File Offset: 0x002031AF
		public override void Notify_BuildingDespawnedOnMap(Building building)
		{
			this.RitualData.psychicRitualToil.Notify_BuildingDespawnedOnMap(this.RitualData.psychicRitual, null, building);
		}

		// Token: 0x040048F3 RID: 18675
		protected PsychicRitualDef def;

		// Token: 0x040048F4 RID: 18676
		protected PsychicRitualRoleAssignments assignments;

		// Token: 0x040048F5 RID: 18677
		protected Func<PsychicRitualToil> psychicRitualToilGenerator;

		// Token: 0x040048F6 RID: 18678
		public int iteration;

		// Token: 0x040048F7 RID: 18679
		public bool removeLordOnCancel = true;
	}
}
