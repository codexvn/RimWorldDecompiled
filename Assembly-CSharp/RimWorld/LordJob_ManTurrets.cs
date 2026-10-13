using System;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001662 RID: 5730
	public class LordJob_ManTurrets : LordJob
	{
		// Token: 0x0600871A RID: 34586 RVA: 0x002858BE File Offset: 0x00283ABE
		public override StateGraph CreateGraph()
		{
			return new StateGraph
			{
				StartingToil = new LordToil_ManClosestTurrets()
			};
		}
	}
}
