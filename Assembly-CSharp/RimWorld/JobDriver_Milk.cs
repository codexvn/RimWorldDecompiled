using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001126 RID: 4390
	public class JobDriver_Milk : JobDriver_GatherAnimalBodyResources
	{
		// Token: 0x17001086 RID: 4230
		// (get) Token: 0x06006B15 RID: 27413 RVA: 0x0021C1B7 File Offset: 0x0021A3B7
		protected override float WorkTotal
		{
			get
			{
				return 400f;
			}
		}

		// Token: 0x06006B16 RID: 27414 RVA: 0x0021C1BE File Offset: 0x0021A3BE
		protected override CompHasGatherableBodyResource GetComp(Pawn animal)
		{
			return animal.TryGetComp<CompMilkable>();
		}
	}
}
