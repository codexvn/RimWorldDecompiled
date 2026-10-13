using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200154C RID: 5452
	public class WorkGiver_Milk : WorkGiver_GatherAnimalBodyResources
	{
		// Token: 0x170013C5 RID: 5061
		// (get) Token: 0x06008171 RID: 33137 RVA: 0x0026FB6F File Offset: 0x0026DD6F
		protected override JobDef JobDef
		{
			get
			{
				return JobDefOf.Milk;
			}
		}

		// Token: 0x06008172 RID: 33138 RVA: 0x0021C1BE File Offset: 0x0021A3BE
		protected override CompHasGatherableBodyResource GetComp(Pawn animal)
		{
			return animal.TryGetComp<CompMilkable>();
		}
	}
}
