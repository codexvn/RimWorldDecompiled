using System;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001655 RID: 5717
	public class LordJob_FleshbeastAssault : LordJob
	{
		// Token: 0x060086C5 RID: 34501 RVA: 0x00283AAC File Offset: 0x00281CAC
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_FleshbeastAssault toil = new LordToil_FleshbeastAssault();
			stateGraph.AddToil(toil);
			return stateGraph;
		}
	}
}
