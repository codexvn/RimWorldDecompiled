using System;

namespace Verse.AI.Group
{
	// Token: 0x02001097 RID: 4247
	public class LordJob_Travel : LordJob
	{
		// Token: 0x06006849 RID: 26697 RVA: 0x0020FCAA File Offset: 0x0020DEAA
		public LordJob_Travel()
		{
		}

		// Token: 0x0600684A RID: 26698 RVA: 0x0020FF97 File Offset: 0x0020E197
		public LordJob_Travel(IntVec3 travelDest)
		{
			this.travelDest = travelDest;
		}

		// Token: 0x0600684B RID: 26699 RVA: 0x0020FFA8 File Offset: 0x0020E1A8
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_Travel lordToil_Travel = new LordToil_Travel(this.travelDest);
			stateGraph.StartingToil = lordToil_Travel;
			LordToil_DefendPoint lordToil_DefendPoint = new LordToil_DefendPoint(false);
			stateGraph.AddToil(lordToil_DefendPoint);
			Transition transition = new Transition(lordToil_Travel, lordToil_DefendPoint, false, true);
			transition.AddTrigger(new Trigger_PawnHarmed(1f, false, null, null, null));
			transition.AddPreAction(new TransitionAction_SetDefendLocalGroup());
			transition.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition, false);
			Transition transition2 = new Transition(lordToil_DefendPoint, lordToil_Travel, false, true);
			transition2.AddTrigger(new Trigger_TicksPassedWithoutHarm(1200));
			transition2.AddPreAction(new TransitionAction_EnsureHaveExitDestination());
			stateGraph.AddTransition(transition2, false);
			return stateGraph;
		}

		// Token: 0x0600684C RID: 26700 RVA: 0x00210050 File Offset: 0x0020E250
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.travelDest, "travelDest", default(IntVec3), false);
		}

		// Token: 0x04004A27 RID: 18983
		private IntVec3 travelDest;
	}
}
