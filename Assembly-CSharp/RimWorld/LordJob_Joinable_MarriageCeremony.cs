using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020016CF RID: 5839
	public class LordJob_Joinable_MarriageCeremony : LordJob_VoluntarilyJoinable
	{
		// Token: 0x1700151A RID: 5402
		// (get) Token: 0x060089D6 RID: 35286 RVA: 0x00292028 File Offset: 0x00290228
		public override bool LostImportantReferenceDuringLoading
		{
			get
			{
				return this.firstPawn == null || this.secondPawn == null;
			}
		}

		// Token: 0x1700151B RID: 5403
		// (get) Token: 0x060089D7 RID: 35287 RVA: 0x00002501 File Offset: 0x00000701
		public override bool AllowStartNewGatherings
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060089D8 RID: 35288 RVA: 0x00291E91 File Offset: 0x00290091
		public LordJob_Joinable_MarriageCeremony()
		{
		}

		// Token: 0x060089D9 RID: 35289 RVA: 0x0029203D File Offset: 0x0029023D
		public LordJob_Joinable_MarriageCeremony(Pawn firstPawn, Pawn secondPawn, IntVec3 spot)
		{
			this.firstPawn = firstPawn;
			this.secondPawn = secondPawn;
			this.spot = spot;
			this.firstPawnSpouseCount = firstPawn.GetSpouseCount(false);
			this.secondPawnSpouseCount = firstPawn.GetSpouseCount(false);
		}

		// Token: 0x060089DA RID: 35290 RVA: 0x00292074 File Offset: 0x00290274
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_Party lordToil_Party = new LordToil_Party(this.spot, GatheringDefOf.Party, 3.5E-05f);
			stateGraph.AddToil(lordToil_Party);
			LordToil_MarriageCeremony lordToil_MarriageCeremony = new LordToil_MarriageCeremony(this.firstPawn, this.secondPawn, this.spot);
			stateGraph.AddToil(lordToil_MarriageCeremony);
			LordToil_Party lordToil_Party2 = new LordToil_Party(this.spot, GatheringDefOf.Party, 3.5E-05f);
			stateGraph.AddToil(lordToil_Party2);
			LordToil_End lordToil_End = new LordToil_End();
			stateGraph.AddToil(lordToil_End);
			Transition transition = new Transition(lordToil_Party, lordToil_MarriageCeremony, false, true);
			transition.AddTrigger(new Trigger_TickCondition(() => this.lord.ticksInToil >= 5000 && this.AreFiancesInPartyArea(), 1));
			transition.AddPreAction(new TransitionAction_Message("MessageMarriageCeremonyStarts".Translate(this.firstPawn.LabelShort, this.secondPawn.LabelShort, this.firstPawn.Named("PAWN1"), this.secondPawn.Named("PAWN2")), MessageTypeDefOf.PositiveEvent, this.firstPawn, null, 1f));
			stateGraph.AddTransition(transition, false);
			Transition transition2 = new Transition(lordToil_MarriageCeremony, lordToil_Party2, false, true);
			transition2.AddTrigger(new Trigger_TickCondition(() => this.firstPawn.relations.DirectRelationExists(PawnRelationDefOf.Spouse, this.secondPawn), 1));
			transition2.AddPreAction(new TransitionAction_Message("MessageNewlyMarried".Translate(this.firstPawn.LabelShort, this.secondPawn.LabelShort, this.firstPawn.Named("PAWN1"), this.secondPawn.Named("PAWN2")), MessageTypeDefOf.PositiveEvent, new TargetInfo(this.spot, base.Map, false), null, 1f));
			transition2.AddPreAction(new TransitionAction_Custom(new Action(this.WeddingSucceeded)));
			stateGraph.AddTransition(transition2, false);
			Transition transition3 = new Transition(lordToil_Party2, lordToil_End, false, true);
			transition3.AddTrigger(new Trigger_TickCondition(new Func<bool>(this.ShouldAfterPartyBeCalledOff), 1));
			transition3.AddTrigger(new Trigger_PawnKilled());
			stateGraph.AddTransition(transition3, false);
			this.afterPartyTimeoutTrigger = new Trigger_TicksPassed(7500);
			Transition transition4 = new Transition(lordToil_Party2, lordToil_End, false, true);
			transition4.AddTrigger(this.afterPartyTimeoutTrigger);
			transition4.AddPreAction(new TransitionAction_Message("MessageMarriageCeremonyAfterPartyFinished".Translate(this.firstPawn.LabelShort, this.secondPawn.LabelShort, this.firstPawn.Named("PAWN1"), this.secondPawn.Named("PAWN2")), MessageTypeDefOf.PositiveEvent, this.firstPawn, null, 1f));
			stateGraph.AddTransition(transition4, false);
			Transition transition5 = new Transition(lordToil_MarriageCeremony, lordToil_End, false, true);
			transition5.AddSource(lordToil_Party);
			transition5.AddTrigger(new Trigger_TickCondition(() => this.lord.ticksInToil >= 120000 && (this.firstPawn.Drafted || this.secondPawn.Drafted || !this.firstPawn.Position.InHorDistOf(this.spot, 4f) || !this.secondPawn.Position.InHorDistOf(this.spot, 4f)), 1));
			transition5.AddPreAction(new TransitionAction_Message("MessageMarriageCeremonyCalledOff".Translate(this.firstPawn.LabelShort, this.secondPawn.LabelShort, this.firstPawn.Named("PAWN1"), this.secondPawn.Named("PAWN2")), MessageTypeDefOf.NegativeEvent, new TargetInfo(this.spot, base.Map, false), null, 1f));
			stateGraph.AddTransition(transition5, false);
			Transition transition6 = new Transition(lordToil_MarriageCeremony, lordToil_End, false, true);
			transition6.AddSource(lordToil_Party);
			transition6.AddTrigger(new Trigger_TickCondition(new Func<bool>(this.ShouldCeremonyBeCalledOff), 1));
			transition6.AddTrigger(new Trigger_PawnKilled());
			transition6.AddPreAction(new TransitionAction_Message("MessageMarriageCeremonyCalledOff".Translate(this.firstPawn.LabelShort, this.secondPawn.LabelShort, this.firstPawn.Named("PAWN1"), this.secondPawn.Named("PAWN2")), MessageTypeDefOf.NegativeEvent, new TargetInfo(this.spot, base.Map, false), null, 1f));
			stateGraph.AddTransition(transition6, false);
			return stateGraph;
		}

		// Token: 0x060089DB RID: 35291 RVA: 0x00292488 File Offset: 0x00290688
		private bool AreFiancesInPartyArea()
		{
			return this.lord.ownedPawns.Contains(this.firstPawn) && this.lord.ownedPawns.Contains(this.secondPawn) && this.firstPawn.Map == base.Map && GatheringsUtility.InGatheringArea(this.firstPawn.Position, this.spot, base.Map) && this.secondPawn.Map == base.Map && GatheringsUtility.InGatheringArea(this.secondPawn.Position, this.spot, base.Map);
		}

		// Token: 0x060089DC RID: 35292 RVA: 0x00292530 File Offset: 0x00290730
		private bool ShouldCeremonyBeCalledOff()
		{
			return this.firstPawn.Destroyed || this.secondPawn.Destroyed || !this.firstPawn.relations.DirectRelationExists(PawnRelationDefOf.Fiance, this.secondPawn) || (this.spot.GetDangerFor(this.firstPawn, base.Map) != Danger.None || this.spot.GetDangerFor(this.secondPawn, base.Map) != Danger.None) || (!GatheringsUtility.AcceptableGameConditionsToContinueGathering(base.Map) || !MarriageCeremonyUtility.FianceCanContinueCeremony(this.firstPawn, this.secondPawn) || !MarriageCeremonyUtility.FianceCanContinueCeremony(this.secondPawn, this.firstPawn));
		}

		// Token: 0x060089DD RID: 35293 RVA: 0x002925E4 File Offset: 0x002907E4
		private bool ShouldAfterPartyBeCalledOff()
		{
			return this.firstPawn.Destroyed || this.secondPawn.Destroyed || (this.firstPawn.Downed || this.secondPawn.Downed) || (this.spot.GetDangerFor(this.firstPawn, base.Map) != Danger.None || this.spot.GetDangerFor(this.secondPawn, base.Map) != Danger.None) || !GatheringsUtility.AcceptableGameConditionsToContinueGathering(base.Map);
		}

		// Token: 0x060089DE RID: 35294 RVA: 0x00292670 File Offset: 0x00290870
		public override float VoluntaryJoinPriorityFor(Pawn p)
		{
			if (this.IsFiance(p))
			{
				if (!MarriageCeremonyUtility.FianceCanContinueCeremony(p, (p == this.firstPawn) ? this.secondPawn : this.firstPawn))
				{
					return 0f;
				}
				return VoluntarilyJoinableLordJobJoinPriorities.MarriageCeremonyFiance;
			}
			else
			{
				if (!this.IsGuest(p))
				{
					return 0f;
				}
				if (!MarriageCeremonyUtility.ShouldGuestKeepAttendingCeremony(p))
				{
					return 0f;
				}
				if (!this.lord.ownedPawns.Contains(p))
				{
					if (this.IsCeremonyAboutToEnd())
					{
						return 0f;
					}
					LordToil_MarriageCeremony lordToil_MarriageCeremony = this.lord.CurLordToil as LordToil_MarriageCeremony;
					IntVec3 intVec;
					if (lordToil_MarriageCeremony != null && !SpectatorCellFinder.TryFindSpectatorCellFor(p, lordToil_MarriageCeremony.Data.spectateRect, base.Map, out intVec, lordToil_MarriageCeremony.Data.spectateRectAllowedSides, 1, null, null, null))
					{
						return 0f;
					}
				}
				return VoluntarilyJoinableLordJobJoinPriorities.MarriageCeremonyGuest;
			}
		}

		// Token: 0x060089DF RID: 35295 RVA: 0x00292738 File Offset: 0x00290938
		public override void ExposeData()
		{
			Scribe_References.Look<Pawn>(ref this.firstPawn, "firstPawn", false);
			Scribe_References.Look<Pawn>(ref this.secondPawn, "secondPawn", false);
			Scribe_Values.Look<int>(ref this.firstPawnSpouseCount, "firstPawnSpouseCount", 0, false);
			Scribe_Values.Look<int>(ref this.secondPawnSpouseCount, "secondPawnSpouseCount", 0, false);
			Scribe_Values.Look<IntVec3>(ref this.spot, "spot", default(IntVec3), false);
		}

		// Token: 0x060089E0 RID: 35296 RVA: 0x002927A5 File Offset: 0x002909A5
		public override string GetReport(Pawn pawn)
		{
			return "LordReportAttendingMarriageCeremony".Translate();
		}

		// Token: 0x060089E1 RID: 35297 RVA: 0x002927B6 File Offset: 0x002909B6
		private bool IsCeremonyAboutToEnd()
		{
			return this.afterPartyTimeoutTrigger.TicksLeft < 1200;
		}

		// Token: 0x060089E2 RID: 35298 RVA: 0x002927CD File Offset: 0x002909CD
		private bool IsFiance(Pawn p)
		{
			return p == this.firstPawn || p == this.secondPawn;
		}

		// Token: 0x060089E3 RID: 35299 RVA: 0x002927E4 File Offset: 0x002909E4
		private bool IsGuest(Pawn p)
		{
			if (!p.RaceProps.Humanlike)
			{
				return false;
			}
			if (p == this.firstPawn || p == this.secondPawn)
			{
				return false;
			}
			HistoryEvent ev = new HistoryEvent(SpouseRelationUtility.GetHistoryEventForSpouseCount(this.firstPawnSpouseCount + 1), p.Named(HistoryEventArgsNames.Doer));
			HistoryEvent ev2 = new HistoryEvent(SpouseRelationUtility.GetHistoryEventForSpouseCount(this.secondPawnSpouseCount + 1), p.Named(HistoryEventArgsNames.Doer));
			return ev.DoerWillingToDo() && ev2.DoerWillingToDo() && (p.Faction == this.firstPawn.Faction || p.Faction == this.secondPawn.Faction);
		}

		// Token: 0x060089E4 RID: 35300 RVA: 0x0029288C File Offset: 0x00290A8C
		private void WeddingSucceeded()
		{
			List<Pawn> ownedPawns = this.lord.ownedPawns;
			for (int i = 0; i < ownedPawns.Count; i++)
			{
				if (ownedPawns[i] != this.firstPawn && ownedPawns[i] != this.secondPawn && ownedPawns[i].needs.mood != null && (this.firstPawn.Position.InHorDistOf(ownedPawns[i].Position, 18f) || this.secondPawn.Position.InHorDistOf(ownedPawns[i].Position, 18f)))
				{
					ownedPawns[i].needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.AttendedWedding, null, null);
				}
			}
		}

		// Token: 0x04005A06 RID: 23046
		public Pawn firstPawn;

		// Token: 0x04005A07 RID: 23047
		public Pawn secondPawn;

		// Token: 0x04005A08 RID: 23048
		private IntVec3 spot;

		// Token: 0x04005A09 RID: 23049
		private int firstPawnSpouseCount;

		// Token: 0x04005A0A RID: 23050
		private int secondPawnSpouseCount;

		// Token: 0x04005A0B RID: 23051
		private Trigger_TicksPassed afterPartyTimeoutTrigger;
	}
}
