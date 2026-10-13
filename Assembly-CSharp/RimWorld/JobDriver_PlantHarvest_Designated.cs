using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001366 RID: 4966
	public class JobDriver_PlantHarvest_Designated : JobDriver_PlantHarvest
	{
		// Token: 0x17001338 RID: 4920
		// (get) Token: 0x060079D8 RID: 31192 RVA: 0x0024C497 File Offset: 0x0024A697
		protected override DesignationDef RequiredDesignation
		{
			get
			{
				return DesignationDefOf.HarvestPlant;
			}
		}
	}
}
