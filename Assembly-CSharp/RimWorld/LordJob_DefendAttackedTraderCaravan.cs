using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200164C RID: 5708
	public class LordJob_DefendAttackedTraderCaravan : LordJob
	{
		// Token: 0x06008699 RID: 34457 RVA: 0x0020FCAA File Offset: 0x0020DEAA
		public LordJob_DefendAttackedTraderCaravan()
		{
		}

		// Token: 0x0600869A RID: 34458 RVA: 0x00283166 File Offset: 0x00281366
		public LordJob_DefendAttackedTraderCaravan(IntVec3 defendSpot)
		{
			this.defendSpot = defendSpot;
		}

		// Token: 0x0600869B RID: 34459 RVA: 0x00283178 File Offset: 0x00281378
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_DefendTraderCaravan lordToil_DefendTraderCaravan = new LordToil_DefendTraderCaravan(this.defendSpot);
			stateGraph.StartingToil = lordToil_DefendTraderCaravan;
			LordToil_ExitMap lordToil_ExitMap = new LordToil_ExitMap(LocomotionUrgency.None, false, false);
			stateGraph.AddToil(lordToil_ExitMap);
			Transition transition = new Transition(lordToil_DefendTraderCaravan, lordToil_ExitMap, false, true);
			transition.AddTrigger(new Trigger_BecameNonHostileToPlayer());
			transition.AddTrigger(new Trigger_TraderAndAllTraderCaravanGuardsLost());
			stateGraph.AddTransition(transition, false);
			return stateGraph;
		}

		// Token: 0x0600869C RID: 34460 RVA: 0x002831D8 File Offset: 0x002813D8
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.defendSpot, "defendSpot", default(IntVec3), false);
		}

		// Token: 0x040057EC RID: 22508
		private IntVec3 defendSpot;
	}
}
