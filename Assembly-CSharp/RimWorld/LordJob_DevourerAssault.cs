using System;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001650 RID: 5712
	public class LordJob_DevourerAssault : LordJob
	{
		// Token: 0x060086AA RID: 34474 RVA: 0x002834F0 File Offset: 0x002816F0
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil toil = new LordToil_DevourerAssault();
			stateGraph.AddToil(toil);
			return stateGraph;
		}
	}
}
