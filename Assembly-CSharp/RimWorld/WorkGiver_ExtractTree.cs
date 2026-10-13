using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020015C1 RID: 5569
	public class WorkGiver_ExtractTree : WorkGiver_RemoveBuilding
	{
		// Token: 0x17001424 RID: 5156
		// (get) Token: 0x060083A4 RID: 33700 RVA: 0x0024AD13 File Offset: 0x00248F13
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.ExtractTree;
			}
		}

		// Token: 0x17001425 RID: 5157
		// (get) Token: 0x060083A5 RID: 33701 RVA: 0x00277F2B File Offset: 0x0027612B
		protected override JobDef RemoveBuildingJob
		{
			get
			{
				return JobDefOf.ExtractTree;
			}
		}
	}
}
