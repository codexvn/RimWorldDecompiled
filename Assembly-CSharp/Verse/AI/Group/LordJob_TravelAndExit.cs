using System;

namespace Verse.AI.Group
{
	// Token: 0x02001098 RID: 4248
	public class LordJob_TravelAndExit : LordJob
	{
		// Token: 0x0600684D RID: 26701 RVA: 0x0020FCAA File Offset: 0x0020DEAA
		public LordJob_TravelAndExit()
		{
		}

		// Token: 0x0600684E RID: 26702 RVA: 0x00210077 File Offset: 0x0020E277
		public LordJob_TravelAndExit(IntVec3 travelDest)
		{
			this.travelDest = travelDest;
		}

		// Token: 0x0600684F RID: 26703 RVA: 0x00210088 File Offset: 0x0020E288
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil startingToil = stateGraph.AttachSubgraph(new LordJob_Travel(this.travelDest).CreateGraph()).StartingToil;
			stateGraph.StartingToil = startingToil;
			LordToil_ExitMap lordToil_ExitMap = new LordToil_ExitMap(LocomotionUrgency.None, false, false);
			stateGraph.AddToil(lordToil_ExitMap);
			stateGraph.AddTransition(new Transition(startingToil, lordToil_ExitMap, false, true)
			{
				triggers = 
				{
					new Trigger_Memo("TravelArrived")
				}
			}, false);
			return stateGraph;
		}

		// Token: 0x06006850 RID: 26704 RVA: 0x002100F4 File Offset: 0x0020E2F4
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.travelDest, "travelDest", default(IntVec3), false);
		}

		// Token: 0x04004A28 RID: 18984
		private IntVec3 travelDest;
	}
}
