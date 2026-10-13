using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200164A RID: 5706
	public class LordJob_CreepJoiner : LordJob
	{
		// Token: 0x0600868D RID: 34445 RVA: 0x0020FCAA File Offset: 0x0020DEAA
		public LordJob_CreepJoiner()
		{
		}

		// Token: 0x0600868E RID: 34446 RVA: 0x00282DAA File Offset: 0x00280FAA
		public LordJob_CreepJoiner(IntVec3 idleSpot, Pawn target)
		{
			this.idleSpot = idleSpot;
			this.target = target;
		}

		// Token: 0x0600868F RID: 34447 RVA: 0x00282DC0 File Offset: 0x00280FC0
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_Travel lordToil_Travel = new LordToil_Travel(this.idleSpot);
			stateGraph.AddToil(lordToil_Travel);
			stateGraph.StartingToil = lordToil_Travel;
			LordToil_WanderNearby lordToil_WanderNearby = new LordToil_WanderNearby();
			stateGraph.AddToil(lordToil_WanderNearby);
			LordToil_End toil = new LordToil_End();
			stateGraph.AddToil(toil);
			Transition transition = new Transition(lordToil_Travel, lordToil_WanderNearby, false, true);
			transition.AddTrigger(new Trigger_Memo("TravelArrived"));
			stateGraph.AddTransition(transition, false);
			Transition transition2 = new Transition(lordToil_Travel, lordToil_WanderNearby, false, true);
			transition2.AddTrigger(new Trigger_Memo("SpokenTo"));
			transition2.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition2, false);
			Transition transition3 = new Transition(lordToil_WanderNearby, toil, false, true);
			transition3.AddSource(lordToil_Travel);
			transition3.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition3, false);
			return stateGraph;
		}

		// Token: 0x06008690 RID: 34448 RVA: 0x00282E84 File Offset: 0x00281084
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.idleSpot, "idleSpot", default(IntVec3), false);
			Scribe_References.Look<Pawn>(ref this.target, "target", false);
		}

		// Token: 0x040057E9 RID: 22505
		private IntVec3 idleSpot;

		// Token: 0x040057EA RID: 22506
		private Pawn target;
	}
}
