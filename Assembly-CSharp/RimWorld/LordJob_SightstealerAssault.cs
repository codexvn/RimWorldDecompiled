using System;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001674 RID: 5748
	public class LordJob_SightstealerAssault : LordJob
	{
		// Token: 0x06008777 RID: 34679 RVA: 0x002874A8 File Offset: 0x002856A8
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil toil = new LordToil_SightstealerAssault();
			stateGraph.AddToil(toil);
			return stateGraph;
		}
	}
}
