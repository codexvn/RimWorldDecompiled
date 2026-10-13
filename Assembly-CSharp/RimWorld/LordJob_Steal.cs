using System;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200167C RID: 5756
	public class LordJob_Steal : LordJob
	{
		// Token: 0x170014C4 RID: 5316
		// (get) Token: 0x060087A6 RID: 34726 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool GuiltyOnDowned
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060087A7 RID: 34727 RVA: 0x0028818C File Offset: 0x0028638C
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_StealCover lordToil_StealCover = new LordToil_StealCover();
			lordToil_StealCover.useAvoidGrid = true;
			stateGraph.AddToil(lordToil_StealCover);
			LordToil_StealCover lordToil_StealCover2 = new LordToil_StealCover();
			lordToil_StealCover2.cover = false;
			lordToil_StealCover2.useAvoidGrid = true;
			stateGraph.AddToil(lordToil_StealCover2);
			Transition transition = new Transition(lordToil_StealCover, lordToil_StealCover2, false, true);
			transition.AddTrigger(new Trigger_TicksPassedAndNoRecentHarm(1200));
			stateGraph.AddTransition(transition, false);
			return stateGraph;
		}
	}
}
