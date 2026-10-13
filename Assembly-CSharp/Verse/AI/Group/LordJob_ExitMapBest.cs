using System;

namespace Verse.AI.Group
{
	// Token: 0x02001095 RID: 4245
	public class LordJob_ExitMapBest : LordJob
	{
		// Token: 0x06006841 RID: 26689 RVA: 0x0020FD93 File Offset: 0x0020DF93
		public LordJob_ExitMapBest()
		{
		}

		// Token: 0x06006842 RID: 26690 RVA: 0x0020FDA2 File Offset: 0x0020DFA2
		public LordJob_ExitMapBest(LocomotionUrgency locomotion, bool canDig = false, bool canDefendSelf = false)
		{
			this.locomotion = locomotion;
			this.canDig = canDig;
			this.canDefendSelf = canDefendSelf;
		}

		// Token: 0x06006843 RID: 26691 RVA: 0x0020FDC8 File Offset: 0x0020DFC8
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_ExitMap lordToil_ExitMap = new LordToil_ExitMap(this.locomotion, this.canDig, false);
			lordToil_ExitMap.useAvoidGrid = true;
			stateGraph.AddToil(lordToil_ExitMap);
			if (this.canDefendSelf)
			{
				LordToil_ExitMapFighting lordToil_ExitMapFighting = new LordToil_ExitMapFighting(LocomotionUrgency.Jog, this.canDig, false);
				stateGraph.AddToil(lordToil_ExitMapFighting);
				Transition transition = new Transition(lordToil_ExitMap, lordToil_ExitMapFighting, false, true);
				transition.AddTrigger(new Trigger_PawnHarmed(1f, false, null, null, null));
				transition.AddPostAction(new TransitionAction_WakeAll());
				transition.AddPostAction(new TransitionAction_EndAllJobs());
				stateGraph.AddTransition(transition, false);
			}
			return stateGraph;
		}

		// Token: 0x06006844 RID: 26692 RVA: 0x0020FE5F File Offset: 0x0020E05F
		public override void ExposeData()
		{
			Scribe_Values.Look<LocomotionUrgency>(ref this.locomotion, "locomotion", LocomotionUrgency.Jog, false);
			Scribe_Values.Look<bool>(ref this.canDig, "canDig", false, false);
			Scribe_Values.Look<bool>(ref this.canDefendSelf, "canDefendSelf", false, false);
		}

		// Token: 0x04004A1E RID: 18974
		private LocomotionUrgency locomotion = LocomotionUrgency.Jog;

		// Token: 0x04004A1F RID: 18975
		private bool canDig;

		// Token: 0x04004A20 RID: 18976
		private bool canDefendSelf;
	}
}
