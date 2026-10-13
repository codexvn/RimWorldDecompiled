using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001658 RID: 5720
	public class LordJob_GorehulkAssault : LordJob
	{
		// Token: 0x060086E0 RID: 34528 RVA: 0x0028466C File Offset: 0x0028286C
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil toil = new LordToil_GorehulkAssault();
			stateGraph.AddToil(toil);
			return stateGraph;
		}

		// Token: 0x060086E1 RID: 34529 RVA: 0x00283118 File Offset: 0x00281318
		public override bool ShouldRemovePawn(Pawn p, PawnLostCondition reason)
		{
			return reason != PawnLostCondition.Incapped;
		}
	}
}
