using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200166B RID: 5739
	public class LordJob_ReimplantXenogerm : LordJob
	{
		// Token: 0x0600874B RID: 34635 RVA: 0x0020FCAA File Offset: 0x0020DEAA
		public LordJob_ReimplantXenogerm()
		{
		}

		// Token: 0x0600874C RID: 34636 RVA: 0x002866D3 File Offset: 0x002848D3
		public LordJob_ReimplantXenogerm(IntVec3 idleSpot, int waitDurationTicks, string inSignalReimplanted)
		{
			this.idleSpot = idleSpot;
			this.waitDurationTicks = waitDurationTicks;
			this.inSignalReimplanted = inSignalReimplanted;
		}

		// Token: 0x0600874D RID: 34637 RVA: 0x002866F0 File Offset: 0x002848F0
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			if (!ModLister.CheckBiotech("Xenogerm reimplantation reward"))
			{
				return stateGraph;
			}
			LordToil_Travel lordToil_Travel = new LordToil_Travel(this.idleSpot);
			stateGraph.AddToil(lordToil_Travel);
			stateGraph.StartingToil = lordToil_Travel;
			LordToil_ReimplantXenogerm lordToil_ReimplantXenogerm = new LordToil_ReimplantXenogerm(this.idleSpot);
			stateGraph.AddToil(lordToil_ReimplantXenogerm);
			LordToil_ExitMapRandom lordToil_ExitMapRandom = new LordToil_ExitMapRandom();
			stateGraph.AddToil(lordToil_ExitMapRandom);
			LordToil_ExitMapAndDefendSelf lordToil_ExitMapAndDefendSelf = new LordToil_ExitMapAndDefendSelf();
			stateGraph.AddToil(lordToil_ExitMapAndDefendSelf);
			Transition transition = new Transition(lordToil_Travel, lordToil_ReimplantXenogerm, false, true);
			transition.AddTrigger(new Trigger_Memo("TravelArrived"));
			transition.canMoveToSameState = true;
			stateGraph.AddTransition(transition, false);
			Transition transition2 = new Transition(lordToil_ReimplantXenogerm, lordToil_ExitMapRandom, false, true);
			transition2.AddSource(lordToil_Travel);
			transition2.AddTrigger(new Trigger_Signal(this.inSignalReimplanted));
			transition2.AddTrigger(new Trigger_TicksPassed(this.waitDurationTicks));
			transition2.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition2, false);
			Transition transition3 = new Transition(lordToil_ReimplantXenogerm, lordToil_ExitMapAndDefendSelf, false, true);
			transition3.AddSource(lordToil_Travel);
			transition3.AddSource(lordToil_ExitMapRandom);
			transition3.AddTrigger(new Trigger_BecamePlayerEnemy());
			transition3.AddTrigger(new Trigger_PawnKilled());
			transition3.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition3, false);
			return stateGraph;
		}

		// Token: 0x0600874E RID: 34638 RVA: 0x00286820 File Offset: 0x00284A20
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.idleSpot, "idleSpot", default(IntVec3), false);
			Scribe_Values.Look<string>(ref this.inSignalReimplanted, "inSignalReimplanted", null, false);
			Scribe_Values.Look<int>(ref this.waitDurationTicks, "waitDurationTicks", 0, false);
		}

		// Token: 0x0400585C RID: 22620
		private IntVec3 idleSpot;

		// Token: 0x0400585D RID: 22621
		private string inSignalReimplanted;

		// Token: 0x0400585E RID: 22622
		private int waitDurationTicks;
	}
}
