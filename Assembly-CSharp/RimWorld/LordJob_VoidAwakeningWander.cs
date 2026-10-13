using System;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001686 RID: 5766
	public class LordJob_VoidAwakeningWander : LordJob
	{
		// Token: 0x060087D3 RID: 34771 RVA: 0x002895C8 File Offset: 0x002877C8
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil toil = new LordToil_VoidAwakeningWander();
			stateGraph.AddToil(toil);
			return stateGraph;
		}
	}
}
