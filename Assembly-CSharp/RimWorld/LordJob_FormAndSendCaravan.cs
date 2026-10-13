using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001656 RID: 5718
	public class LordJob_FormAndSendCaravan : LordJob
	{
		// Token: 0x170014A3 RID: 5283
		// (get) Token: 0x060086C7 RID: 34503 RVA: 0x00283ACB File Offset: 0x00281CCB
		public bool GatheringItemsNow
		{
			get
			{
				return this.lord.CurLordToil == this.gatherItems;
			}
		}

		// Token: 0x170014A4 RID: 5284
		// (get) Token: 0x060086C8 RID: 34504 RVA: 0x00002501 File Offset: 0x00000701
		public override bool AllowStartNewGatherings
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170014A5 RID: 5285
		// (get) Token: 0x060086C9 RID: 34505 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool NeverInRestraints
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170014A6 RID: 5286
		// (get) Token: 0x060086CA RID: 34506 RVA: 0x00002501 File Offset: 0x00000701
		public override bool AddFleeToil
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170014A7 RID: 5287
		// (get) Token: 0x060086CB RID: 34507 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool ManagesRopableAnimals
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170014A8 RID: 5288
		// (get) Token: 0x060086CC RID: 34508 RVA: 0x00283AE0 File Offset: 0x00281CE0
		public IntVec3 ExitSpot
		{
			get
			{
				return this.exitSpot;
			}
		}

		// Token: 0x170014A9 RID: 5289
		// (get) Token: 0x060086CD RID: 34509 RVA: 0x00283AE8 File Offset: 0x00281CE8
		public string Status
		{
			get
			{
				LordToil curLordToil = this.lord.CurLordToil;
				if (curLordToil == this.gatherItems)
				{
					return "FormingCaravanStatus_GatheringItems".Translate();
				}
				if (curLordToil == this.gatherItems_pause)
				{
					return "FormingCaravanStatus_GatheringItems_Pause".Translate();
				}
				if (curLordToil == this.gatherDownedPawns)
				{
					return "FormingCaravanStatus_GatheringDownedPawns".Translate();
				}
				if (curLordToil == this.gatherDownedPawns_pause)
				{
					return "FormingCaravanStatus_GatheringDownedPawns_Pause".Translate();
				}
				if (curLordToil == this.gatherAnimals)
				{
					return "FormingCaravanStatus_GatheringAnimals".Translate();
				}
				if (curLordToil == this.gatherAnimals_pause)
				{
					return "FormingCaravanStatus_GatheringAnimals_Pause".Translate();
				}
				if (curLordToil == this.collectAnimals)
				{
					return "FormingCaravanStatus_GatheringAnimals".Translate();
				}
				if (curLordToil == this.collectAnimals_pause)
				{
					return "FormingCaravanStatus_GatheringAnimals_Pause".Translate();
				}
				if (curLordToil == this.leave)
				{
					return "FormingCaravanStatus_Leaving".Translate();
				}
				if (curLordToil == this.leave_pause)
				{
					return "FormingCaravanStatus_Leaving_Pause".Translate();
				}
				return "FormingCaravanStatus_Waiting".Translate();
			}
		}

		// Token: 0x060086CE RID: 34510 RVA: 0x0020FCAA File Offset: 0x0020DEAA
		public LordJob_FormAndSendCaravan()
		{
		}

		// Token: 0x060086CF RID: 34511 RVA: 0x00283C0A File Offset: 0x00281E0A
		public LordJob_FormAndSendCaravan(List<TransferableOneWay> transferables, List<Pawn> downedPawns, IntVec3 meetingPoint, IntVec3 exitSpot, PlanetTile startingTile, PlanetTile destinationTile)
		{
			this.transferables = transferables;
			this.downedPawns = downedPawns;
			this.meetingPoint = meetingPoint;
			this.exitSpot = exitSpot;
			this.startingTile = startingTile;
			this.destinationTile = destinationTile;
		}

		// Token: 0x060086D0 RID: 34512 RVA: 0x00283C40 File Offset: 0x00281E40
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			this.gatherAnimals = new LordToil_PrepareCaravan_GatherAnimals(this.meetingPoint);
			stateGraph.AddToil(this.gatherAnimals);
			this.gatherAnimals_pause = new LordToil_PrepareCaravan_Pause();
			stateGraph.AddToil(this.gatherAnimals_pause);
			this.gatherItems = new LordToil_PrepareCaravan_GatherItems(this.meetingPoint);
			stateGraph.AddToil(this.gatherItems);
			this.gatherItems_pause = new LordToil_PrepareCaravan_Pause();
			stateGraph.AddToil(this.gatherItems_pause);
			this.gatherDownedPawns = new LordToil_PrepareCaravan_GatherDownedPawns(this.meetingPoint, this.exitSpot);
			stateGraph.AddToil(this.gatherDownedPawns);
			this.gatherDownedPawns_pause = new LordToil_PrepareCaravan_Pause();
			stateGraph.AddToil(this.gatherDownedPawns_pause);
			LordToil_PrepareCaravan_Wait lordToil_PrepareCaravan_Wait = new LordToil_PrepareCaravan_Wait(this.meetingPoint);
			stateGraph.AddToil(lordToil_PrepareCaravan_Wait);
			LordToil_PrepareCaravan_Pause lordToil_PrepareCaravan_Pause = new LordToil_PrepareCaravan_Pause();
			stateGraph.AddToil(lordToil_PrepareCaravan_Pause);
			this.collectAnimals = new LordToil_PrepareCaravan_CollectAnimals(this.exitSpot);
			stateGraph.AddToil(this.collectAnimals);
			this.collectAnimals_pause = new LordToil_PrepareCaravan_Pause();
			stateGraph.AddToil(this.collectAnimals_pause);
			this.leave = new LordToil_PrepareCaravan_Leave(this.exitSpot);
			stateGraph.AddToil(this.leave);
			this.leave_pause = new LordToil_PrepareCaravan_Pause();
			stateGraph.AddToil(this.leave_pause);
			LordToil_End lordToil_End = new LordToil_End();
			stateGraph.AddToil(lordToil_End);
			Transition transition = new Transition(this.gatherAnimals, this.gatherItems, false, true);
			transition.AddTrigger(new Trigger_Memo("AllAnimalsGathered"));
			stateGraph.AddTransition(transition, false);
			Transition transition2 = new Transition(this.gatherItems, this.gatherDownedPawns, false, true);
			transition2.AddTrigger(new Trigger_Memo("AllItemsGathered"));
			transition2.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition2, false);
			Transition transition3 = new Transition(this.gatherDownedPawns, lordToil_PrepareCaravan_Wait, false, true);
			transition3.AddTrigger(new Trigger_Memo("AllDownedPawnsGathered"));
			transition3.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition3, false);
			Transition transition4 = new Transition(lordToil_PrepareCaravan_Wait, this.collectAnimals, false, true);
			transition4.AddTrigger(new Trigger_NoPawnsVeryTiredAndSleeping(0f));
			transition4.AddPostAction(new TransitionAction_WakeAll());
			stateGraph.AddTransition(transition4, false);
			Transition transition5 = new Transition(this.collectAnimals, this.leave, false, true);
			transition5.AddTrigger(new Trigger_Memo("AllAnimalsCollected"));
			stateGraph.AddTransition(transition5, false);
			Transition transition6 = new Transition(this.leave, this.collectAnimals, false, true);
			transition6.AddTrigger(new Trigger_Memo("AddedRopeAnimal"));
			transition6.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition6, false);
			Transition transition7 = new Transition(this.leave, this.collectAnimals, false, true);
			transition7.AddTrigger(new Trigger_Memo("RemovedPawn"));
			transition7.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition7, false);
			Transition transition8 = new Transition(this.leave, lordToil_End, false, true);
			transition8.AddTrigger(new Trigger_Memo("ReadyToExitMap"));
			transition8.AddPreAction(new TransitionAction_Custom(new Action(this.SendCaravan)));
			stateGraph.AddTransition(transition8, false);
			Transition transition9 = this.PauseTransition(this.gatherAnimals, this.gatherAnimals_pause);
			stateGraph.AddTransition(transition9, false);
			Transition transition10 = this.UnpauseTransition(this.gatherAnimals_pause, this.gatherAnimals);
			stateGraph.AddTransition(transition10, false);
			Transition transition11 = this.PauseTransition(this.gatherItems, this.gatherItems_pause);
			stateGraph.AddTransition(transition11, false);
			Transition transition12 = this.UnpauseTransition(this.gatherItems_pause, this.gatherItems);
			stateGraph.AddTransition(transition12, false);
			Transition transition13 = this.PauseTransition(this.gatherDownedPawns, this.gatherDownedPawns_pause);
			stateGraph.AddTransition(transition13, false);
			Transition transition14 = this.UnpauseTransition(this.gatherDownedPawns_pause, this.gatherDownedPawns);
			stateGraph.AddTransition(transition14, false);
			Transition transition15 = this.PauseTransition(this.collectAnimals, this.collectAnimals_pause);
			stateGraph.AddTransition(transition15, false);
			Transition transition16 = this.UnpauseTransition(this.collectAnimals_pause, this.collectAnimals);
			stateGraph.AddTransition(transition16, false);
			Transition transition17 = this.PauseTransition(this.leave, this.leave_pause);
			stateGraph.AddTransition(transition17, false);
			Transition transition18 = this.UnpauseTransition(this.leave_pause, this.collectAnimals);
			stateGraph.AddTransition(transition18, false);
			Transition transition19 = this.PauseTransition(lordToil_PrepareCaravan_Wait, lordToil_PrepareCaravan_Pause);
			stateGraph.AddTransition(transition19, false);
			Transition transition20 = this.UnpauseTransition(lordToil_PrepareCaravan_Pause, lordToil_PrepareCaravan_Wait);
			stateGraph.AddTransition(transition20, false);
			Transition transition21 = new Transition(this.gatherAnimals, this.leave, false, true);
			transition21.AddTrigger(new Trigger_Memo("ForceDepartNow"));
			transition21.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition21, false);
			transition21 = new Transition(this.gatherItems, this.leave, false, true);
			transition21.AddTrigger(new Trigger_Memo("ForceDepartNow"));
			transition21.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition21, false);
			transition21 = new Transition(this.gatherDownedPawns, this.leave, false, true);
			transition21.AddTrigger(new Trigger_Memo("ForceDepartNow"));
			transition21.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition21, false);
			transition21 = new Transition(lordToil_PrepareCaravan_Wait, this.leave, false, true);
			transition21.AddTrigger(new Trigger_Memo("ForceDepartNow"));
			transition21.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition21, false);
			transition21 = new Transition(this.collectAnimals, this.leave, false, true);
			transition21.AddTrigger(new Trigger_Memo("ForceDepartNow"));
			transition21.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition21, false);
			transition21 = new Transition(this.gatherAnimals_pause, this.leave, false, true);
			transition21.AddTrigger(new Trigger_Memo("ForceDepartNow"));
			transition21.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition21, false);
			transition21 = new Transition(this.gatherItems_pause, this.leave, false, true);
			transition21.AddTrigger(new Trigger_Memo("ForceDepartNow"));
			transition21.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition21, false);
			transition21 = new Transition(this.gatherDownedPawns_pause, this.leave, false, true);
			transition21.AddTrigger(new Trigger_Memo("ForceDepartNow"));
			transition21.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition21, false);
			transition21 = new Transition(lordToil_PrepareCaravan_Pause, this.leave, false, true);
			transition21.AddTrigger(new Trigger_Memo("ForceDepartNow"));
			transition21.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition21, false);
			transition21 = new Transition(this.collectAnimals_pause, this.leave, false, true);
			transition21.AddTrigger(new Trigger_Memo("ForceDepartNow"));
			transition21.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition21, false);
			return stateGraph;
		}

		// Token: 0x060086D1 RID: 34513 RVA: 0x002842D0 File Offset: 0x002824D0
		public override void LordJobTick()
		{
			base.LordJobTick();
			for (int i = this.downedPawns.Count - 1; i >= 0; i--)
			{
				if (this.downedPawns[i].Destroyed)
				{
					this.downedPawns.RemoveAt(i);
				}
				else if (!this.downedPawns[i].Downed)
				{
					this.lord.AddPawn(this.downedPawns[i]);
					this.downedPawns.RemoveAt(i);
				}
			}
		}

		// Token: 0x060086D2 RID: 34514 RVA: 0x00284352 File Offset: 0x00282552
		public override string GetReport(Pawn pawn)
		{
			return "LordReportFormingCaravan".Translate();
		}

		// Token: 0x060086D3 RID: 34515 RVA: 0x00284364 File Offset: 0x00282564
		private Transition PauseTransition(LordToil from, LordToil to)
		{
			Transition transition = new Transition(from, to, false, true);
			transition.AddPreAction(new TransitionAction_Message("MessageCaravanFormationPaused".Translate(), MessageTypeDefOf.NegativeEvent, () => this.lord.ownedPawns.FirstOrDefault((Pawn x) => x.InMentalState), null, 1f));
			transition.AddTrigger(new Trigger_MentalState());
			transition.AddPostAction(new TransitionAction_EndAllJobs());
			return transition;
		}

		// Token: 0x060086D4 RID: 34516 RVA: 0x002843C4 File Offset: 0x002825C4
		private Transition UnpauseTransition(LordToil from, LordToil to)
		{
			Transition transition = new Transition(from, to, false, true);
			transition.AddPreAction(new TransitionAction_Message("MessageCaravanFormationUnpaused".Translate(), MessageTypeDefOf.SilentInput, null, 1f, null));
			transition.AddTrigger(new Trigger_NoMentalState());
			transition.AddPostAction(new TransitionAction_EndAllJobs());
			return transition;
		}

		// Token: 0x060086D5 RID: 34517 RVA: 0x00284418 File Offset: 0x00282618
		public override void ExposeData()
		{
			Scribe_Collections.Look<TransferableOneWay>(ref this.transferables, "transferables", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<Pawn>(ref this.downedPawns, "downedPawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<IntVec3>(ref this.meetingPoint, "meetingPoint", default(IntVec3), false);
			Scribe_Values.Look<IntVec3>(ref this.exitSpot, "exitSpot", default(IntVec3), false);
			Scribe_Values.Look<PlanetTile>(ref this.startingTile, "startingTile", default(PlanetTile), false);
			Scribe_Values.Look<PlanetTile>(ref this.destinationTile, "destinationTile", default(PlanetTile), false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.downedPawns.RemoveAll((Pawn x) => x.DestroyedOrNull());
			}
		}

		// Token: 0x060086D6 RID: 34518 RVA: 0x002844EC File Offset: 0x002826EC
		private void SendCaravan()
		{
			this.caravanSent = true;
			CaravanFormingUtility.FormAndCreateCaravan(this.lord.ownedPawns.Concat(from x in this.downedPawns
			where JobGiver_PrepareCaravan_GatherDownedPawns.IsDownedPawnNearExitPoint(x, this.exitSpot)
			select x), this.lord.faction, base.Map.Tile, this.startingTile, this.destinationTile);
		}

		// Token: 0x060086D7 RID: 34519 RVA: 0x00284550 File Offset: 0x00282750
		public override void Notify_PawnAdded(Pawn p)
		{
			base.Notify_PawnAdded(p);
			Pawn_JobTracker jobs = p.jobs;
			if (jobs != null)
			{
				jobs.SetFormingCaravanTick(false);
			}
			ReachabilityUtility.ClearCacheFor(p);
			if (AnimalPenUtility.NeedsToBeManagedByRope(p))
			{
				if (p.roping.IsRopedByPawn)
				{
					p.roping.BreakAllRopes();
				}
				this.lord.ReceiveMemo("AddedRopeAnimal");
			}
		}

		// Token: 0x060086D8 RID: 34520 RVA: 0x002845AC File Offset: 0x002827AC
		public override void Notify_PawnLost(Pawn p, PawnLostCondition condition)
		{
			base.Notify_PawnLost(p, condition);
			ReachabilityUtility.ClearCacheFor(p);
			if (!this.caravanSent)
			{
				if ((condition == PawnLostCondition.Incapped || condition == PawnLostCondition.Killed) && p.Downed)
				{
					this.downedPawns.Add(p);
				}
				CaravanFormingUtility.RemovePawnFromCaravan(p, this.lord, false);
				this.lord.ReceiveMemo("RemovedPawn");
			}
		}

		// Token: 0x060086D9 RID: 34521 RVA: 0x00284608 File Offset: 0x00282808
		public override bool CanOpenAnyDoor(Pawn p)
		{
			return !p.RaceProps.FenceBlocked;
		}

		// Token: 0x04005800 RID: 22528
		public List<TransferableOneWay> transferables;

		// Token: 0x04005801 RID: 22529
		public List<Pawn> downedPawns;

		// Token: 0x04005802 RID: 22530
		private IntVec3 meetingPoint;

		// Token: 0x04005803 RID: 22531
		private IntVec3 exitSpot;

		// Token: 0x04005804 RID: 22532
		private PlanetTile startingTile;

		// Token: 0x04005805 RID: 22533
		private PlanetTile destinationTile;

		// Token: 0x04005806 RID: 22534
		private bool caravanSent;

		// Token: 0x04005807 RID: 22535
		private LordToil gatherAnimals;

		// Token: 0x04005808 RID: 22536
		private LordToil gatherAnimals_pause;

		// Token: 0x04005809 RID: 22537
		private LordToil gatherItems;

		// Token: 0x0400580A RID: 22538
		private LordToil gatherItems_pause;

		// Token: 0x0400580B RID: 22539
		private LordToil gatherDownedPawns;

		// Token: 0x0400580C RID: 22540
		private LordToil gatherDownedPawns_pause;

		// Token: 0x0400580D RID: 22541
		private LordToil collectAnimals;

		// Token: 0x0400580E RID: 22542
		private LordToil collectAnimals_pause;

		// Token: 0x0400580F RID: 22543
		private LordToil leave;

		// Token: 0x04005810 RID: 22544
		private LordToil leave_pause;

		// Token: 0x04005811 RID: 22545
		public const float CustomWakeThreshold = 0.5f;
	}
}
