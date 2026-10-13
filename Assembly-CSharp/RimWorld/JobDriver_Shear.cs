using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001134 RID: 4404
	public class JobDriver_Shear : JobDriver_GatherAnimalBodyResources
	{
		// Token: 0x17001091 RID: 4241
		// (get) Token: 0x06006B76 RID: 27510 RVA: 0x0021D34B File Offset: 0x0021B54B
		protected override float WorkTotal
		{
			get
			{
				return 1700f;
			}
		}

		// Token: 0x06006B77 RID: 27511 RVA: 0x0021D352 File Offset: 0x0021B552
		protected override CompHasGatherableBodyResource GetComp(Pawn animal)
		{
			return animal.TryGetComp<CompShearable>();
		}
	}
}
