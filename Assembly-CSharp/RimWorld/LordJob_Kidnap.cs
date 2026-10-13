using System;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200165F RID: 5727
	public class LordJob_Kidnap : LordJob
	{
		// Token: 0x170014AE RID: 5294
		// (get) Token: 0x06008708 RID: 34568 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool GuiltyOnDowned
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600870A RID: 34570 RVA: 0x00285798 File Offset: 0x00283998
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_KidnapCover lordToil_KidnapCover = new LordToil_KidnapCover();
			lordToil_KidnapCover.useAvoidGrid = true;
			stateGraph.AddToil(lordToil_KidnapCover);
			LordToil_KidnapCover lordToil_KidnapCover2 = new LordToil_KidnapCover();
			lordToil_KidnapCover2.cover = false;
			lordToil_KidnapCover2.useAvoidGrid = true;
			stateGraph.AddToil(lordToil_KidnapCover2);
			Transition transition = new Transition(lordToil_KidnapCover, lordToil_KidnapCover2, false, true);
			transition.AddTrigger(new Trigger_TicksPassed(1200));
			stateGraph.AddTransition(transition, false);
			return stateGraph;
		}

		// Token: 0x0600870B RID: 34571 RVA: 0x000026EA File Offset: 0x000008EA
		public override void ExposeData()
		{
		}
	}
}
