using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001367 RID: 4967
	public class JobDriver_PlantCut_Designated : JobDriver_PlantCut
	{
		// Token: 0x17001339 RID: 4921
		// (get) Token: 0x060079DA RID: 31194 RVA: 0x0024C4A6 File Offset: 0x0024A6A6
		protected override DesignationDef RequiredDesignation
		{
			get
			{
				return DesignationDefOf.CutPlant;
			}
		}
	}
}
