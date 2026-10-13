using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001654 RID: 5716
	public class LordJob_ExitOnShuttle : LordJob
	{
		// Token: 0x170014A2 RID: 5282
		// (get) Token: 0x060086BD RID: 34493 RVA: 0x00283983 File Offset: 0x00281B83
		public override bool AddFleeToil
		{
			get
			{
				return this.addFleeToil;
			}
		}

		// Token: 0x060086BE RID: 34494 RVA: 0x0028398B File Offset: 0x00281B8B
		public LordJob_ExitOnShuttle()
		{
		}

		// Token: 0x060086BF RID: 34495 RVA: 0x0028399A File Offset: 0x00281B9A
		public LordJob_ExitOnShuttle(Thing shuttle, bool addFleeToil = true)
		{
			this.shuttle = shuttle;
			this.addFleeToil = addFleeToil;
		}

		// Token: 0x060086C0 RID: 34496 RVA: 0x00283118 File Offset: 0x00281318
		public override bool ShouldRemovePawn(Pawn p, PawnLostCondition reason)
		{
			return reason != PawnLostCondition.Incapped;
		}

		// Token: 0x060086C1 RID: 34497 RVA: 0x002839B8 File Offset: 0x00281BB8
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			if (!ModLister.CheckRoyalty("Shuttle crash rescue"))
			{
				return stateGraph;
			}
			LordToil_Wait lordToil_Wait = new LordToil_Wait(true);
			stateGraph.AddToil(lordToil_Wait);
			stateGraph.StartingToil = lordToil_Wait;
			LordToil_EnterShuttleOrLeave lordToil_EnterShuttleOrLeave = new LordToil_EnterShuttleOrLeave(this.shuttle, LocomotionUrgency.Sprint, false, false);
			stateGraph.AddToil(lordToil_EnterShuttleOrLeave);
			Transition transition = new Transition(lordToil_Wait, lordToil_EnterShuttleOrLeave, false, true);
			transition.AddPreAction(new TransitionAction_Custom(new Action(this.InitializeLoading)));
			transition.AddTrigger(new Trigger_Custom((TriggerSignal signal) => signal.type == TriggerSignalType.Tick && this.shuttle.Spawned));
			stateGraph.AddTransition(transition, false);
			return stateGraph;
		}

		// Token: 0x060086C2 RID: 34498 RVA: 0x00283A45 File Offset: 0x00281C45
		private void InitializeLoading()
		{
			if (!this.shuttle.TryGetComp<CompTransporter>().LoadingInProgressOrReadyToLaunch)
			{
				TransporterUtility.InitiateLoading(Gen.YieldSingle<CompTransporter>(this.shuttle.TryGetComp<CompTransporter>()));
			}
		}

		// Token: 0x060086C3 RID: 34499 RVA: 0x00283A6F File Offset: 0x00281C6F
		public override void ExposeData()
		{
			Scribe_References.Look<Thing>(ref this.shuttle, "shuttle", false);
			Scribe_Values.Look<bool>(ref this.addFleeToil, "addFleeToil", false, false);
		}

		// Token: 0x040057FE RID: 22526
		public Thing shuttle;

		// Token: 0x040057FF RID: 22527
		private bool addFleeToil = true;
	}
}
