using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001688 RID: 5768
	public class LordJob_WanderNest : LordJob
	{
		// Token: 0x060087DA RID: 34778 RVA: 0x00289698 File Offset: 0x00287898
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_WanderNest toil = new LordToil_WanderNest();
			stateGraph.AddToil(toil);
			return stateGraph;
		}

		// Token: 0x060087DB RID: 34779 RVA: 0x00283118 File Offset: 0x00281318
		public override bool ShouldRemovePawn(Pawn p, PawnLostCondition reason)
		{
			return reason != PawnLostCondition.Incapped;
		}
	}
}
