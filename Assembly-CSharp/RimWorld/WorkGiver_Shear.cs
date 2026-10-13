using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200154F RID: 5455
	public class WorkGiver_Shear : WorkGiver_GatherAnimalBodyResources
	{
		// Token: 0x170013C9 RID: 5065
		// (get) Token: 0x06008183 RID: 33155 RVA: 0x0026FDEF File Offset: 0x0026DFEF
		protected override JobDef JobDef
		{
			get
			{
				return JobDefOf.Shear;
			}
		}

		// Token: 0x06008184 RID: 33156 RVA: 0x0021D352 File Offset: 0x0021B552
		protected override CompHasGatherableBodyResource GetComp(Pawn animal)
		{
			return animal.TryGetComp<CompShearable>();
		}
	}
}
