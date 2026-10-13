using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020015C8 RID: 5576
	public class WorkGiver_FillIn : WorkGiver_RemoveBuilding
	{
		// Token: 0x1700142E RID: 5166
		// (get) Token: 0x060083CA RID: 33738 RVA: 0x00223CAF File Offset: 0x00221EAF
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.FillIn;
			}
		}

		// Token: 0x1700142F RID: 5167
		// (get) Token: 0x060083CB RID: 33739 RVA: 0x00278736 File Offset: 0x00276936
		protected override JobDef RemoveBuildingJob
		{
			get
			{
				return JobDefOf.FillIn;
			}
		}
	}
}
